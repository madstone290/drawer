## EndPoints
| EndPoint | 내용 |
|--|--|
| localhost:11000 | web blue |
| localhost:12000 | web green |
| localhost:11100 | api blue |
| localhost:12100 | api green |

## Files
|경로|내용|
|--|--|
| ~/drawer/api/publish | 외부에서 api 게시물을 전송할 디렉토리 |
| ~/drawer/api/blue | blue api 파일 |
| ~/drawer/api/green | green api 파일 |
| ~/drawer/web/publish | 외부에서 web 게시물을 전송할 디렉토리 |
| ~/drawer/web/blue | blue web 파일 |
| ~/drawer/web/green | green web 파일 |
| /etc/nginx/sites-available/drawer-api-blue.conf| api blue 서버 nginx 설정 | |
| /etc/nginx/sites-available/drawer-api-green.conf| api green 서버 nginx 설정 |
| /etc/nginx/sites-available/drawer-api-live.conf| api live 서버 nginx 설정 | |
| /etc/nginx/sites-available/drawer-web-blue.conf| web blue 서버 nginx 설정 | |
| /etc/nginx/sites-available/drawer-web-green.conf| web green 서버 nginx 설정 |
| /etc/nginx/sites-available/drawer-web-live.conf| web live 서버 nginx 설정 | |

### blue/green 활성화 상태 변경
활성화된 컬러에 맞는 링크경로를 변경한다.
/etc/nginx/sites-enabled/drawer-upstream.conf의 링크경로를 blue or green으로 변경한다.
```
# blue를 활성화 상태로 변경
sudo ln -sf /etc/nginx/sites-available/drawer-blue.conf /etc/nginx/sites-enabled/drawer-upstream.conf

# green을 활성화 상태로 변경
sudo ln -sf /etc/nginx/sites-available/drawer-green.conf /etc/nginx/sites-enabled/drawer-upstream.conf

# 설정 리로드
sudo systemctl reload nginx.
```
