import type { Enum } from './Enum';
import type { Schema } from './Schema';

export interface Model extends Schema {
    name: string;
    export: 'reference' | 'generic'| 'genric-interface' | 'alias' | 'enum' | 'array' | 'dictionary' | 'interface' | 'one-of' | 'any-of' | 'all-of' | 'none';
    type: string;
    base: string;
    template: string | null;
    link: Model | null;
    description: string | null;
    deprecated?: boolean;
    default?: string;
    imports: string[];
    enum: Enum[];
    enums: Model[];
    properties: Model[];
}
