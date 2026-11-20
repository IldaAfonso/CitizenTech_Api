param([string]$Base = "http://localhost:5000")

function Chamar($metodo, $url, $corpo) {
    try {
        if ($corpo) { $r = Invoke-WebRequest -Uri $url -Method $metodo -ContentType "application/json" -Body $corpo -UseBasicParsing }
        else        { $r = Invoke-WebRequest -Uri $url -Method $metodo -UseBasicParsing }
        $status = [int]$r.StatusCode; $texto = $r.Content
    } catch {
        $status = [int]$_.Exception.Response.StatusCode
        $texto = $_.ErrorDetails.Message
        if (-not $texto) {
            try { $texto = (New-Object IO.StreamReader($_.Exception.Response.GetResponseStream())).ReadToEnd() } catch { $texto = "" }
        }
    }
    Write-Host ("{0} {1} -> {2}" -f $metodo, $url, $status) -ForegroundColor Cyan
    if ($texto) { Write-Host $texto }
    Write-Host ""
    return [pscustomobject]@{ Status = $status; Texto = $texto }
}

$sufixo = Get-Random -Minimum 1000 -Maximum 9999

Write-Host "=== 1) Cuidador (esperado 201) ===" -ForegroundColor Yellow
$c = Chamar "POST" "$Base/api/cuidadores" '{"nome":"Carla Souza","telefone":"11988880001","email":"carla@email.com","tipo":"FAMILIAR"}'
$cid = ($c.Texto | ConvertFrom-Json).id

Write-Host "=== 2) Idoso ligado ao cuidador - relacao 1:N (esperado 201) ===" -ForegroundColor Yellow
$i = Chamar "POST" "$Base/api/idosos" ('{"cuidadorId":' + $cid + ',"nome":"Maria Aparecida","dataNascimento":"1944-03-12T00:00:00","telefone":"11911110001","endereco":"Rua das Flores, 10","codigoDispositivo":"ESP32-' + $sufixo + '"}')
$iid = ($i.Texto | ConvertFrom-Json).id

Write-Host "=== 3) Medicamento (esperado 201) ===" -ForegroundColor Yellow
$m = Chamar "POST" "$Base/api/medicamentos" ('{"idosoId":' + $iid + ',"nome":"Losartana","dosagem":"50 mg","horarioPrevisto":"08:00"}')

Write-Host "=== 4) Evento (esperado 201) ===" -ForegroundColor Yellow
$e = Chamar "POST" "$Base/api/eventos" ('{"idosoId":' + $iid + ',"tipo":"BOTAO_EMERGENCIA","status":"PENDENTE","observacao":"Teste no video"}')
$eid = ($e.Texto | ConvertFrom-Json).id

Write-Host "=== 5) GET lista (200) ===" -ForegroundColor Yellow
$null = Chamar "GET" "$Base/api/eventos" $null
Write-Host "=== 6) GET por id (200) ===" -ForegroundColor Yellow
$null = Chamar "GET" "$Base/api/eventos/$eid" $null
Write-Host "=== 7) PUT (204) ===" -ForegroundColor Yellow
$null = Chamar "PUT" "$Base/api/eventos/$eid" ('{"id":' + $eid + ',"idosoId":' + $iid + ',"tipo":"BOTAO_EMERGENCIA","status":"RESOLVIDO","observacao":"Resolvido"}')
Write-Host "=== 8) ERRO: POST invalido (400 com JSON) ===" -ForegroundColor Yellow
$null = Chamar "POST" "$Base/api/eventos" '{"idosoId":0,"tipo":"XXX"}'
Write-Host "=== 9) ERRO: id inexistente (404 com JSON) ===" -ForegroundColor Yellow
$null = Chamar "GET" "$Base/api/eventos/99999" $null
Write-Host "=== 10) DELETE (204) ===" -ForegroundColor Yellow
$null = Chamar "DELETE" "$Base/api/eventos/$eid" $null
Write-Host "Pronto. Confira no Oracle: SELECT * FROM NET_CUIDADOR; SELECT * FROM NET_IDOSO;" -ForegroundColor Green