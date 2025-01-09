/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ApiData } from './ApiData';
import type { ApiLinks } from './ApiLinks';
import type { Metadata } from './Metadata';
export type Response<T> = {
    links?: ApiLinks;
    data: Array<ApiData<T>>;
    metadata: Metadata;
    message?: string | null;
};
