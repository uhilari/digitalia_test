import { HttpEventType, HttpInterceptorFn } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs';

export const encryptInterceptor: HttpInterceptorFn = (req, next) => {
  const newReq = req.clone({
    body: btoa(JSON.stringify(req.body)),
    responseType: 'text'
  });
  return next(newReq)
    .pipe(map((r: any) => {
      if (r.type === HttpEventType.Response && r.body) {
        r.body = JSON.parse(atob(r.body));
      }
      return r;
    }), catchError(err => {
      err.error = JSON.parse(atob(err.error));
      throw err;
    }));
};
