from http.server import HTTPServer, SimpleHTTPRequestHandler

handler = SimpleHTTPRequestHandler
httpd = HTTPServer(('localhost', 8888), handler)

httpd.serve_forever()