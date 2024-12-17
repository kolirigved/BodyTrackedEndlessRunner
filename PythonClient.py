import socket

def send_data(data):
    host, port = "127.0.0.1", 25001

    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    try:
        sock.connect((host, port))
        sock.sendto(str.encode(str(data)), port)
        print(data)

    except Exception as e:
        print(e)

    finally:
        sock.close()
    

