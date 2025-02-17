/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ApiData } from './ApiData';
import type { Metadata } from './Metadata';
export type Response<T> = {
	data?: Array<ApiData<T>> | null;
	metadata?: Metadata;
	message?: string | null;
};

