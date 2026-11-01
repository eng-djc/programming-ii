"""Windows integration check. Creates its own disposable LocalDB catalog, never Zara.
Run from any directory after building Cherkessia in Release. Standard library only.
"""
import datetime
import html
import http.cookiejar
import os
from pathlib import Path
import re
import socket
import subprocess
import time
import urllib.error
import urllib.parse
import urllib.request
import uuid

if os.name != 'nt':
    raise SystemExit('This integration check requires Windows and SQL Server LocalDB.')
project = Path(__file__).resolve().parents[1] / 'Cherkessia'
dll = project / 'bin/Release/net8.0/Cherkessia.dll'
if not dll.exists():
    raise SystemExit('Build Cherkessia.csproj in Release first.')
with socket.socket() as sock:
    sock.bind(('127.0.0.1', 0))
    port = sock.getsockname()[1]
base = f'http://127.0.0.1:{port}'
catalog = 'Cherkessia_Verification_' + uuid.uuid4().hex
print('Isolated test database:', catalog, flush=True)
env = dict(os.environ, ASPNETCORE_ENVIRONMENT='Production', ASPNETCORE_URLS=base,
           ConnectionStrings__BloggingDatabase=f'Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={catalog};Integrated Security=True;')
client = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))
checks = 0
process = None
log = (project.parent / 'Verification/http-server.log').open('w', encoding='utf-8')

def request(path, data=None):
    body = urllib.parse.urlencode(data).encode() if data is not None else None
    try:
        with client.open(base + path, body, timeout=90) as r:
            return r.status, html.unescape(r.read().decode())
    except urllib.error.HTTPError as e:
        return e.code, html.unescape(e.read().decode())

def check(ok, label):
    global checks
    if not ok:
        raise AssertionError(label)
    checks += 1
    print('PASS', label, flush=True)

def get(path):
    status, body = request(path)
    check(status == 200, 'GET ' + path)
    check(not re.search(r'<script\b|\bon\w+\s*=|javascript:', body, re.I), 'No executable JS: ' + path)
    return body

def token(path):
    body = get(path)
    match = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', body)
    check(match is not None, 'Antiforgery token: ' + path)
    return match.group(1)

def post(path, data, token_path=None):
    return request(path, dict(data, __RequestVerificationToken=token(token_path or path)))

def start():
    global process
    process = subprocess.Popen(['dotnet', str(dll)], cwd=project, env=env, stdout=log, stderr=subprocess.STDOUT)
    for _ in range(60):
        if process.poll() is not None:
            raise RuntimeError('Application exited; see http-server.log')
        try:
            if request('/')[0] == 200:
                return
        except OSError:
            pass
        time.sleep(1)
    raise RuntimeError('Application startup timeout')

def stop():
    if process is not None and process.poll() is None:
        process.terminate()
        try:
            process.wait(timeout=15)
        except subprocess.TimeoutExpired:
            process.kill()
            process.wait()

name = 'Volcán / prueba ñ'
query = urllib.parse.urlencode({'nombre': name})
volcano = {'Nombre': name, 'Provincia': 'Cartago', 'Activo': 'false', 'Gases': 'Poco probable', 'InstrumentoCodigo': '101'}
instrument = {'Codigo': '101', 'Nombre': 'Sensor de prueba', 'FechaRegistro': f'{datetime.date.today().year - 6}-01-01', 'Precio': '10.50', 'Garantia': '5'}
try:
    start()
    for path in ['/', '/Home/Privacy', '/Home/Error', '/Instrumento', '/Volcan']:
        get(path)
    body = get('/Volcan/Create')
    check('Primero registre un instrumento' in body, 'Empty instrument catalog')
    check(request('/Instrumento/Create', instrument)[0] == 400, 'Create without token rejected')
    check(request('/Instrumento/Details/999999')[0] == 404, 'Missing instrument')
    invalid = dict(instrument, Garantia='6')
    status, body = post('/Instrumento/Create', invalid)
    check(status == 200 and 'La garantía debe ser de 1 a 5 años' in body, 'Server warranty validation')
    check(request('/Instrumento/Details/101')[0] == 404, 'Invalid form was not inserted')
    status, body = post('/Instrumento/Create', instrument)
    check(status == 200 and 'Sensor de prueba' in body, 'Create instrument')
    check('Fuera de garantía' in get('/Instrumento/Details/101'), 'Expired warranty')
    status, body = post('/Instrumento/Create', instrument)
    check(status == 200 and 'Ya existe un instrumento' in body, 'Duplicate code')
    status, _ = post('/Instrumento/Edit/101', dict(instrument, Codigo='102'), '/Instrumento/Edit/101')
    check(status == 400, 'Immutable instrument key')
    status, body = post('/Instrumento/Edit/101', dict(instrument, Nombre='Sensor actualizado'), '/Instrumento/Edit/101')
    check(status == 200 and 'Sensor actualizado' in body, 'Update instrument')
    status, body = post('/Volcan/Create', dict(volcano, Provincia='Otro'))
    check(status == 200 and 'siete provincias' in body, 'Reject forged province')
    status, body = post('/Volcan/Create', dict(volcano, InstrumentoCodigo='999999'))
    check(status == 200 and 'instrumento seleccionado no existe' in body, 'Reject missing FK')
    check(request('/Volcan/Details?' + query)[0] == 404, 'Invalid volcano was not inserted')
    status, body = post('/Volcan/Create', dict(volcano, IRV='8'))
    check(status == 200 and 'Riesgo Muy Bajo' in body, 'Create volcano, ignore client IRV')
    body = get('/Volcan/Details?' + query)
    check('1 — Riesgo Muy Bajo' in body, 'IRV minimum is one')
    status, body = post('/Volcan/Create', volcano)
    check(status == 200 and 'Ya existe un volcán' in body, 'Duplicate volcano')
    status, _ = post('/Volcan/Edit?' + query, dict(volcano, Nombre='Otro'), '/Volcan/Edit?' + query)
    check(status == 400, 'Immutable volcano key')
    status, body = post('/Volcan/Edit?' + query, dict(volcano, Activo='true', Gases='Muy probable'), '/Volcan/Edit?' + query)
    check(status == 200 and 'Riesgo Medio' in body, 'Update volcano recalculates IRV')
    check('5 — Riesgo Medio' in get('/Volcan/Details?' + query), 'Recalculated IRV five')
    stop()
    start()
    check('5 — Riesgo Medio' in get('/Volcan/Details?' + query), 'Persists across application restart')
    get('/Volcan/Delete?' + query)
    check(request('/Volcan/Details?' + query)[0] == 200, 'GET delete does not delete')
    check(request('/Volcan/Delete?' + query, {})[0] == 400, 'Delete without token rejected')
    status, _ = post('/Volcan/Delete?' + query, {}, '/Volcan/Delete?' + query)
    check(status == 200 and request('/Volcan/Details?' + query)[0] == 404, 'Delete volcano')
    check(request('/Instrumento/Details/101')[0] == 200, 'Deleting volcano retains instrument')
    check(request('/Instrumento/Delete/101')[0] == 404, 'Instrument deletion not exposed')
    print(f'PASS: {checks} HTTP/integration checks.', flush=True)
finally:
    stop()
    log.close()
# The isolated database remains available for inspection. On GitHub runners it
# disappears with the VM; on a local PC delete only this named test catalog after review.
