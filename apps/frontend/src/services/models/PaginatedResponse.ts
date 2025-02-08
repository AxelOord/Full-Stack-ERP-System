/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ApiData } from './ApiData';
import type { Metadata } from './Metadata';
import type { PaginationLinks } from './PaginationLinks';
export type PaginatedResponse<T> = {
    links: PaginationLinks;
    data?: Array<ApiData<T>> | null;
    metadata?: Metadata;
    message?: string | null;
};

