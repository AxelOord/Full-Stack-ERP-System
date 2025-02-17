import type { OpenApi } from '../interfaces/OpenApi';

export const getServer = (openApi: OpenApi): string => {
    const server = openApi.servers?.[0];
    const variables = server?.variables || {};
    let url = server?.url || '';
    for (const variable in variables) {
        if (variables.hasOwnProperty(variable)) {
            url = url.replace(`{${variable}}`, variables[variable].default);
        }
    }

    url = url.replace('0.0.0.0', 'localhost'); // fix for local development, chrome doesn't like 0.0.0.0

    return url.replace(/\/$/g, '');
};
