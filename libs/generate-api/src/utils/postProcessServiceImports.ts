import type { Service } from '../client/interfaces/Service';
import { sort } from './sort';
import { unique } from './unique';

/**
 * Set unique imports, sorted by name
 * @param service
 */
export const postProcessServiceImports = (service: Service): string[] => {
    let serviceImports = service.imports.filter(unique).sort(sort)

    serviceImports = serviceImports.map((serviceImport) => {
        if (serviceImport.includes('Of')) {
            const [baseName, genericPart] = serviceImport.split('Of');
            return serviceImport = baseName
        }
        return serviceImport;
    }).flat();

    return serviceImports;
};
