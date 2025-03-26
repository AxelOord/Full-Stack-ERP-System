import { ColumnDef } from "@tanstack/react-table";
import { ApiData } from "@/services/models/ApiData";
import { Column } from "@/services/models/Column";
import { DataTableColumnHeader, FilterOperator } from "./data-table-column-header";

export const createColumns = <T extends object>(
  fields: Column[],
  onFilterApply: (field: string, value: string, operator: FilterOperator) => void,
  getFilterValue: (field: string, operator: FilterOperator) => string | null
): ColumnDef<ApiData<T>>[] => {
  const columns: ColumnDef<ApiData<T>>[] = fields.map((field) => ({
    accessorKey: `attributes.${String(field.field)}`,
    header: ({ column }) => (
      <DataTableColumnHeader
        column={column}
        title={field.key!}
        field={field.field!}
        onFilterApply={onFilterApply}
        getFilterValue={getFilterValue}
      >
        <DataTableColumnHeader.Sort />
        {('filterable' in field ? field.filterable !== false : true) && (
          <>
            <DataTableColumnHeader.Filter />
            <DataTableColumnHeader.Separator />
          </>
        )}
        <DataTableColumnHeader.Visibility />
      </DataTableColumnHeader>
    ),
    enableSorting: field.sortable ?? true,
    // for now check field.filterable if it exists, otherwise default to true
    // TODO: add to Column model in the backend and remove ternary
    enableFiltering: 'filterable' in field ? field.filterable : true,
  }));

  return columns;
};