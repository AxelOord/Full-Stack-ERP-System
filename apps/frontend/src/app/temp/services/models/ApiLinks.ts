/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import { Response } from "@/services/models/Response";
import { requestFromUrl as __request } from '../core/request';
import { ArticleDto } from "./ArticleDto";

export type ApiLinks = {
    self: string;
    related?: string | null;
    next?: string | null;
    prev?: string | null;
}

export async function callLink<T>(link: string): Promise<Response<T> | null> {
    return await __request(link);
}

