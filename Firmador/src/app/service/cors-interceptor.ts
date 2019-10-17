import {Injectable} from "@angular/core";
import {HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";

@Injectable()
export class CorsInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {

    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    const newHeader = req.clone({
      headers: req.headers
                  .set('Access-Control-Allow-Origin', '*')
                  .set('Access-Control-Allow-Credentials', 'true')
    });
    // send cloned request with header to the next handler.
    return next.handle(newHeader);
  }
}