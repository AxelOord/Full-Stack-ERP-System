/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ApiLinks } from './ApiLinks';
import type { Metadata } from './Metadata';
export type Response<T> = {
    links?: ApiLinks;
    data?: Array<T> | null;
    metadata?: Metadata;
    message?: string | null;
};

