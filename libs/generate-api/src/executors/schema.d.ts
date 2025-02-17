import { HttpClient } from "../HttpClient";
import { Indent } from "../Indent";

export interface GenerateExecutorSchema {
    input: string;
    output: string;
    httpClient?:HttpClient;
    clientName?: string;
    useOptions?: boolean;
    useUnionTypes?: boolean;
    exportCore?: boolean;
    exportServices?: boolean;
    exportModels?: boolean;
    exportSchemas?: boolean;
    indent?: Indent;
    postfixServices?: string;
    postfixModels?: string;
    request?: string;
    write?: boolean;
    verbose?: boolean;
}
