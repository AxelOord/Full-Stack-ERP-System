import type { Service } from '../client/interfaces/Service';
import { deduplicate } from './deduplicate';

export const sortServicesByName = (services: Service[]): Service[] => {
    const uniqueServices = deduplicate(services, service => service.name);

    return uniqueServices.sort((a, b) => {
        const nameA = a.name.toLowerCase();
        const nameB = b.name.toLowerCase();
        return nameA.localeCompare(nameB, 'en');
    });
};
