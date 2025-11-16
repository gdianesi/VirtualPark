import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, retry, throwError } from 'rxjs';
import environment from '../../environments/environment';

export default abstract class GenericApiRepository {
    protected readonly baseUrl: string;

    constructor(
        protected readonly resourcePath: string,
        protected readonly http: HttpClient,
    ) {
        const base = environment.apiBaseUrl.replace(/\/+$/, '');
        const res  = resourcePath.replace(/^\/+|\/+$/g, '');
        this.baseUrl = res ? `${base}/${res}` : base;
    }

    protected requestOptions(includeAuth = true) {
        const token = localStorage.getItem('token');
        const headers: Record<string, string> = {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        };
        if (includeAuth && token) headers['Authorization'] = `Bearer ${token}`;
        return { headers: new HttpHeaders(headers) };
    }

    private buildUrl(id?: string, additionalPath = ''): string {
        const base = this.baseUrl.replace(/\/+$/, '');
        const cleanPath = additionalPath.replace(/^\/+/, '');
        const cleanId = id ? `${id}`.replace(/^\/+/, '') : '';

        if (cleanPath && cleanId) return `${base}/${cleanPath}/${cleanId}`;
        if (cleanPath) return `${base}/${cleanPath}`;
        if (cleanId) return `${base}/${cleanId}`;
        return base;
    }


    public getAll<T>(additionalPath = ''): Observable<T> {
        return this.http.get<T>(this.buildUrl(additionalPath), this.requestOptions()).pipe(
            retry(2), catchError(this.handleError)
        );
    }

    public getById<T>(id: string, includeAuth = true, additionalPath = ''): Observable<T> {
        return this.http.get<T>(this.buildUrl(id, additionalPath), this.requestOptions(includeAuth)).pipe(
            retry(2), catchError(this.handleError)
        );
    }

    public create<T>(body: any, includeAuth = true, additionalPath = ''): Observable<T> {
        return this.http.post<T>(this.buildUrl(additionalPath), body, this.requestOptions(includeAuth)).pipe(
            retry(2),catchError(this.handleError)
        );
    }

    public updateById<T>(id: string, body: any, includeAuth = true, additionalPath = ''): Observable<T> {
        return this.http.put<T>(this.buildUrl(id, additionalPath), body, this.requestOptions(includeAuth)).pipe(
            retry(2), catchError(this.handleError)
        );
    }

    public deleteById<T>(id: string, includeAuth = true, additionalPath = ''): Observable<T> {
        return this.http.delete<T>(this.buildUrl(id, additionalPath), this.requestOptions(includeAuth)).pipe(
            retry(2), catchError(this.handleError)
        );
    }
    public patchById<T>(id: string, body: any, includeAuth = true, additionalPath = ''): Observable<T> {
        return this.http.patch<T>(this.buildUrl(id, additionalPath), body, this.requestOptions(includeAuth)).pipe(
            retry(2), catchError(this.handleError)
        );
    }

private handleError(error: HttpErrorResponse) {

    console.error(
        `Backend returned code ${error.status}, body was: `,
        error.error
    );

    return throwError(() => error);
}

    protected getWithParams<T>(
        paramsObj: Record<string, string>,
        includeAuth = true,
        suffix = ''
    ): Observable<T> {
        let params = new HttpParams();
        for (const [k, v] of Object.entries(paramsObj)) params = params.set(k, v);

        const url = suffix ? `${this.baseUrl}/${suffix}` : this.baseUrl;
        const opts = this.requestOptions(includeAuth);
        return this.http.get<T>(url, { ...opts, params });
    }

}