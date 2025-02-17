import type { Model } from '../client/interfaces/Model';
import { sort } from './sort';
import { unique } from './unique';

/**
 * Set unique imports, sorted by name
 * @param model The model that is post-processed
 */
export const postProcessModelImports = (model: Model): string[] => {
    let modelImports = model.imports
        .filter(unique)
        .sort(sort)
        .filter(name => model.name !== name);

    modelImports = modelImports.map((modelImport) => {
        if (modelImport.includes('Of')) {
            const match = modelImport.match(/([A-Za-z]+)(Of[A-Za-z]+)+/);
            if (match) {
                return match[1]; // Extract only the first part (base name)
            }
        }
        return modelImport;
    });

    return modelImports;
};
