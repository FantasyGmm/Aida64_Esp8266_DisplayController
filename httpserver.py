import sys
import os
import hashlib
from http.server import HTTPServer, BaseHTTPRequestHandler

host = ("0.0.0.0", 8266)
file = ""

class Resquest(BaseHTTPRequestHandler):
    def do_GET(self):
        fp = open(file, "rb")
        data = fp.read()
        fp.close()
        md5 = hashlib.md5(data).hexdigest()
        print(md5)
        print(self.headers)
        
        espmd5 = self.headers["x-ESP8266-sketch-md5"]

        if espmd5 is None or md5 == espmd5:
            print("bad request")
            return
        

        print("ESP8266 MD5:%s" % self.headers["x-ESP8266-sketch-md5"])
        
        self.send_response(200)
        self.send_header("Content-type", "application/octet-stream")
        self.send_header("Content-Disposition", "attachment;filename=update.bin")
        self.send_header("Content-Length", str(os.path.getsize(file)))
        self.end_headers()
        self.wfile.write(data)

if __name__ == "__main__":
    server = HTTPServer(host, Resquest)
    file=sys.argv[1]
    print("Starting server, listen at: %s:%s" % host)
    print("update bin is %s" % file)
    server.serve_forever()
