/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { PaginatedResponse } from '../models/PaginatedResponse';
import type { Response } from '../models/Response';
import type { SupplierDto } from '../models/SupplierDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class SuppliersService {
    /**
     * @param id
     * @returns Response<SupplierDto> OK
     * @throws ApiError
     */
    public static getApiSuppliers(
        id: string,
    ): CancelablePromise<Response<SupplierDto>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/suppliers/{id}',
            path: {
                'id': id,
            },
            errors: {
                400: `Bad Request`,
                404: `Not Found`,
                500: `Internal Server Error`,
            },
        });
    }
    /**
     * @param id
     * @returns void
     * @throws ApiError
     */
    public static deleteApiSuppliers(
        id: string,
    ): CancelablePromise<void> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/suppliers/{id}',
            path: {
                'id': id,
            },
            errors: {
                400: `Bad Request`,
                404: `Not Found`,
            },
        });
    }
    /**
     * @param pageNumber
     * @param pageSize
     * @param filterProperty
     * @param filterValue
     * @param orderBy
     * @param isDescending
     * @returns PaginatedResponseOfSupplierDto OK
     * @throws ApiError
     */
    public static getApiSuppliers1(
        pageNumber?: number,
        pageSize?: number,
        filterProperty?: string,
        filterValue?: string,
        orderBy?: string,
        isDescending?: boolean,
    ): CancelablePromise<PaginatedResponse<SupplierDto>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/suppliers',
            query: {
                'PageNumber': pageNumber,
                'PageSize': pageSize,
                'FilterProperty': filterProperty,
                'FilterValue': filterValue,
                'OrderBy': orderBy,
                'IsDescending': isDescending,
            },
            errors: {
                400: `Bad Request`,
            },
        });
    }
}
