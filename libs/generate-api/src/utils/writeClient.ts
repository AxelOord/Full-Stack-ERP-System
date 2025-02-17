import { resolve } from 'path';
const path = require("path");
const fs = require("fs");

import type { Client } from '../client/interfaces/Client';
import type { Indent } from '../Indent';
import { mkdir, rmdir } from './fileSystem';
import { isDefined } from './isDefined';
import { isSubDirectory } from './isSubdirectory';
import type { Templates } from './registerHandlebarTemplates';
import { writeClientClass } from './writeClientClass';
import { writeClientCore } from './writeClientCore';
import { writeClientIndex } from './writeClientIndex';
import { writeClientModels } from './writeClientModels';
import { writeClientSchemas } from './writeClientSchemas';
import { writeClientServices } from './writeClientServices';

/**
 * Write our OpenAPI client, using the given templates at the given output
 * @param client Client object with all the models, services, etc.
 * @param templates Templates wrapper with all loaded Handlebars templates
 * @param output The relative location of the output directory
 * @param useOptions Use options or arguments functions
 * @param useUnionTypes Use union types instead of enums
 * @param exportCore Generate core client classes
 * @param exportServices Generate services
 * @param exportModels Generate models
 * @param exportSchemas Generate schemas
 * @param exportSchemas Generate schemas
 * @param indent Indentation options (4, 2 or tab)
 * @param postfixServices Service name postfix
 * @param postfixModels Model name postfix
 * @param clientName Custom client class name
 * @param request Path to custom request file
 */
export const writeClient = async (
    client: Client,
    templates: Templates,
    output: string,
    useOptions: boolean,
    useUnionTypes: boolean,
    exportCore: boolean,
    exportServices: boolean,
    exportModels: boolean,
    exportSchemas: boolean,
    indent: Indent,
    postfixServices: string,
    postfixModels: string,
    clientName?: string,
    request?: string
): Promise<void> => {
    function findGitRoot() {
        let dir = process.cwd();
        while (!fs.existsSync(path.join(dir, ".git")) && dir !== path.dirname(dir)) {
            dir = path.dirname(dir);
        }
        return dir;
    } // because i'm lazy and hacky
    
    const gitRoot = findGitRoot();
    
    const outputPath = output.startsWith(".") // if the output path is relative
        ? path.resolve(gitRoot, output)
        : path.resolve(process.cwd(), output);

    const outputPathCore = resolve(outputPath, 'core');
    const outputPathModels = resolve(outputPath, 'models');
    const outputPathSchemas = resolve(outputPath, 'schemas');
    const outputPathServices = resolve(outputPath, 'services');

    if (!isSubDirectory(process.cwd(), output)) {
        throw new Error(`Output folder is not a subdirectory of the current working directory`);
    }

    if (exportCore) {
        await rmdir(outputPathCore);
        await mkdir(outputPathCore);
        await writeClientCore(client, templates, outputPathCore, indent, clientName, request);
    }

    if (exportServices) {
        await rmdir(outputPathServices);
        await mkdir(outputPathServices);
        await writeClientServices(
            client.services,
            templates,
            outputPathServices,
            useUnionTypes,
            useOptions,
            indent,
            postfixServices,
            clientName
        );
    }

    if (exportSchemas) {
        await rmdir(outputPathSchemas);
        await mkdir(outputPathSchemas);
        await writeClientSchemas(client.models, templates, outputPathSchemas, useUnionTypes, indent);
    }

    if (exportModels) {
        await rmdir(outputPathModels);
        await mkdir(outputPathModels);
        await writeClientModels(client.models, templates, outputPathModels, useUnionTypes, indent);
    }

    if (isDefined(clientName)) {
        await mkdir(outputPath);
        await writeClientClass(client, templates, outputPath, clientName, indent, postfixServices);
    }

    if (exportCore || exportServices || exportSchemas || exportModels) {
        await mkdir(outputPath);
        await writeClientIndex(
            client,
            templates,
            outputPath,
            useUnionTypes,
            exportCore,
            exportServices,
            exportModels,
            exportSchemas,
            postfixServices,
            postfixModels,
            clientName
        );
    }
};
