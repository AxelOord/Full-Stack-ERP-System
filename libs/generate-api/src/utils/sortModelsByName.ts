import type { Model } from '../client/interfaces/Model';
import { deduplicate } from './deduplicate';

export const sortModelsByName = (models: Model[]): Model[] => {
    const uniqueModels = deduplicate(models, model => model.name);
    
    return uniqueModels.sort((a, b) => {
        const nameA = a.name.toLowerCase();
        const nameB = b.name.toLowerCase();
        
        return nameA.localeCompare(nameB, 'en');
    });
};
