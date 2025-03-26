/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateSupplierRequest } from '../models/CreateSupplierRequest';
import type { FilterCriteria } from '../models/FilterCriteria';
import type { FilterGroup } from '../models/FilterGroup';
import type { PaginatedResponse } from '../models/PaginatedResponse';
import type { Response } from '../models/Response';
import type { SupplierDto } from '../models/SupplierDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class SuppliersService {
	/**
	 * @param requestBody
	 * @returns any Created
	 * @throws ApiError
	 */
	public static postApiSuppliers(
		requestBody: CreateSupplierRequest,
	): CancelablePromise<any> {
		return __request(OpenAPI, {
			method: 'POST',
			url: '/api/suppliers',
			body: requestBody,
			mediaType: 'application/json',
			errors: {
				400: `Bad Request`,
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
	 * @param filterDefinitionRootGroupCriteria
	 * @param filterDefinitionRootGroupGroups
	 * @param filterDefinitionRootGroupLogicalOperator
	 * @param rawFilters
	 * @returns PaginatedResponse<SupplierDto> OK
	 * @throws ApiError
	 */
	public static getApiSuppliers(
		pageNumber?: number,
		pageSize?: number,
		filterProperty?: string,
		filterValue?: string,
		orderBy?: string,
		isDescending?: boolean,
		filterDefinitionRootGroupCriteria?: Array<FilterCriteria>,
		filterDefinitionRootGroupGroups?: Array<FilterGroup>,
		filterDefinitionRootGroupLogicalOperator?: string,
		rawFilters?: Record<string, string>,
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
				'FilterDefinition.RootGroup.Criteria': filterDefinitionRootGroupCriteria,
				'FilterDefinition.RootGroup.Groups': filterDefinitionRootGroupGroups,
				'FilterDefinition.RootGroup.LogicalOperator': filterDefinitionRootGroupLogicalOperator,
				'RawFilters': rawFilters,
			},
			errors: {
				400: `Bad Request`,
			},
		});
	}
	/**
	 * @param id
	 * @returns Response<SupplierDto> OK
	 * @throws ApiError
	 */
	public static getApiSuppliers1(
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
}
