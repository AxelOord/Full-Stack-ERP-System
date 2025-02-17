import { EOL } from 'os';

import { Indent } from '../Indent';

export const formatIndentation = (s: string, indent: Indent): string => {
    let lines = s.split(/\r?\n/); // Cross-platform safe splitting

    lines = lines.map(line => {
        switch (indent) {
            case Indent.SPACE_4:
                return line.replace(/\t/g, '    ');
            case Indent.SPACE_2:
                return line.replace(/\t/g, '  ');
            case Indent.TAB:
                return line
            default:
                return line;
        }
    });

    const content = lines.join('\n');
    const result = `${content}\n`;

    return result;
};


