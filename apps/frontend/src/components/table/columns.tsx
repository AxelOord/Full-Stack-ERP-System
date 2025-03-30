import { ColumnDef } from "@tanstack/react-table";
import { ApiData } from "@/services/models/ApiData";
import { Column } from "@/services/models/Column";
import { ChevronRight } from "lucide-react";
import { DataTableColumnHeader, FilterOperator } from "@/components/table/data-table-column-header";

export const createColumns = <T extends object>(
  fields: Column[],
  onFilterApply: (field: string, value: string, operator: FilterOperator) => void,
  getFilterValue: (field: string, operator: FilterOperator) => string | null,
  hasExpandableRows?: boolean = false
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

  if (hasExpandableRows) {
    return [
      {
        header: () => <span className="w-10 sr-only">Expand</span>,
        id: 'expand',
        enableResizing: false,
        enableSorting: false,
        enableFiltering: false,
        meta: {
          isExpandColumn: true
        },
        cell: ({ row }) => {
          return (
            <div 
            className="flex justify-center items-center w-full h-full"
            role="expand" 
            >
              {row.getCanExpand() ? (
                <button
                  onClick={row.getToggleExpandedHandler()}
                  className="w-7 h-7 bg-orange flex items-center justify-center rounded transition-transform duration-300"
                  aria-label={row.getIsExpanded() ? "Collapse row" : "Expand row"}
                >
                  <ChevronRight
                    size={20}
                    className={`text-white transform transition-transform duration-300 ${row.getIsExpanded() ? 'rotate-90' : ''}`}
                  />
                </button>
              ) : null}
            </div>
          );
        },
      },
      ...columns
    ];
  }

  return columns;
};