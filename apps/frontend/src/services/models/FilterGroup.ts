/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { FilterCriteria } from './FilterCriteria';
export type FilterGroup = {
	criteria?: Array<FilterCriteria>;
	groups?: Array<FilterGroup>;
	logicalOperator?: string;
};

