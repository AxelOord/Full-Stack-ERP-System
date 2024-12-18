import { ColumnDef } from "@tanstack/react-table";
import { ApiData } from "@/services/models/ApiData";
import { Column } from "@/services/models/Column";
import { DataTableColumnHeader } from "./data-table-column-header"

export const createColumns = <T extends object>(fields: Column[]): ColumnDef<ApiData<T>>[] => {
    const columns: ColumnDef<ApiData<T>>[] = fields.map((field) => ({
        accessorKey: `attributes.${String(field.field)}`,
        header: ({ column }) => (
            <DataTableColumnHeader column={column} title={field.key!} />
          ),
    }));

    return columns;
};