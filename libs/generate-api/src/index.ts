import { Indent } from './Indent';
import { parse as parseV3 } from './openApi';
import { getOpenApiSpec } from './utils/getOpenApiSpec';
import { isString } from './utils/isString';
import { postProcessClient } from './utils/postProcessClient';
import { registerHandlebarTemplates } from './utils/registerHandlebarTemplates';
import { writeClient } from './utils/writeClient';

export { Indent } from './Indent';

export type Options = {
    input: string | Record<string, any>;
    output: string;
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
};

/**
 * Generate the OpenAPI client. This method will read the OpenAPI specification and based on the
 * given language it will generate the client, including the typed models, validation schemas,
 * service layer, etc.
 * @param input The relative location of the OpenAPI spec
 * @param output The relative location of the output directory
 * @param clientName Custom client class name
 * @param useOptions Use options or arguments functions
 * @param useUnionTypes Use union types instead of enums
 * @param exportCore Generate core client classes
 * @param exportServices Generate services
 * @param exportModels Generate models
 * @param exportSchemas Generate schemas
 * @param indent Indentation options (4, 2 or tab)
 * @param postfixServices Service name postfix
 * @param postfixModels Model name postfix
 * @param request Path to custom request file
 * @param write Write the files to disk (true or false)
 */
export const generate = async ({
    input,
    output,
    clientName,
    useOptions = false,
    useUnionTypes = false,
    exportCore = true,
    exportServices = true,
    exportModels = true,
    exportSchemas = false,
    indent = Indent.SPACE_4,
    postfixServices = 'Service',
    postfixModels = '',
    request,
    write = true,
}: Options): Promise<void> => {
    const openApi = isString(input) ? await getOpenApiSpec(input) : input;
    const templates = registerHandlebarTemplates({
        useUnionTypes,
        useOptions,
    });

    const client = parseV3(openApi);
    const clientFinal = postProcessClient(client);
    if (!write) return;
    await writeClient(
        clientFinal,
        templates,
        output,
        useOptions,
        useUnionTypes,
        exportCore,
        exportServices,
        exportModels,
        exportSchemas,
        indent,
        postfixServices,
        postfixModels,
        clientName,
        request
    );
};

export default {
    generate,
};
