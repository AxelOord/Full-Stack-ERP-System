"use client"

import {
  ColumnDef,
  ColumnFiltersState,
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable,
  VisibilityState,
  getExpandedRowModel
} from "@tanstack/react-table"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "./table"
import React, { Suspense } from "react"
import { createColumns } from "@/components/table/columns"
import { ApiData, Link, PaginatedResponse } from "@/services";
import { Skeleton } from "@/components/ui/skeleton"
import Loader from "@/components/shared/loader"
import TableToolbar from "@/components/table/table-toolbar"
import Pagination from "@/components/table/pagination"
import { useDataTableFilters } from "@/hooks/use-data-table-filters";
import { BaseDto } from "@/services/models/BaseDto"

interface DataTableContextValue<TDto extends BaseDto> {
  table: ReturnType<typeof useReactTable<TDto>> | null;
  data: PaginatedResponse<TDto> | null;
  isLoading: boolean;
  columns: ColumnDef<ApiData<TDto>>[];
  onSearch: ((searchTerm: string) => void) | null;
  onPaginate: (link?: Link) => void;
  onFilter?: (activeFilters: Record<string, { operator: string; value: string }[]>) => void;
  updateContext: (newState: Partial<DataTableContextState<TDto>>) => void;
  filters: ReturnType<typeof useDataTableFilters>;
  skeletonRowCount?: number;
}

interface DataTableContextState<TDto extends BaseDto> {
  table: ReturnType<typeof useReactTable<TDto>> | null;
  data: PaginatedResponse<TDto> | null;
  isLoading: boolean;
  columns: ColumnDef<ApiData<TDto>>[];
  onSearch: ((searchTerm: string) => void) | null;
  onPaginate: (link?: Link) => void;
  onFilter?: (activeFilters: Record<string, { operator: string; value: string }[]>) => void;
  skeletonRowCount?: number;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const DataTableContext = React.createContext<any>(undefined);

function useDataTable<TDto extends BaseDto>(): DataTableContextValue<TDto> {
  const context = React.useContext(DataTableContext);
  if (!context) {
    throw new Error("useDataTable must be used within a DataTable");
  }
  return context as DataTableContextValue<TDto>;
}

interface DataTableProps {
  children?: React.ReactNode;
}

function DataTable<TDto extends object>({ children }: DataTableProps) {
  const [state, setState] = React.useState<DataTableContextState<TDto>>({
    table: null,
    data: null,
    isLoading: true,
    columns: [],
    onSearch: null,
    onPaginate: () => { },
    onFilter: undefined,
  });

  const updateContext = React.useCallback((newState: Partial<DataTableContextState<TDto>>) => {
    setState(prevState => ({ ...prevState, ...newState }));
  }, []);

  const getActiveFiltersRef = React.useRef<(() => Record<string, { operator: string; value: string }[]>) | null>(null);
  const onFilterRef = React.useRef<typeof state.onFilter>(state.onFilter);

  React.useEffect(() => {
    onFilterRef.current = state.onFilter;
  }, [state.onFilter]);

  const handleFilter = React.useCallback((activeFilters: Record<string, { operator: string; value: string }[]>) => {
    if (onFilterRef.current) {
      onFilterRef.current(activeFilters);
    }
  }, []);

  const filterFunctions = useDataTableFilters(handleFilter);

  React.useEffect(() => {
    getActiveFiltersRef.current = filterFunctions.getActiveFilters;
  }, [filterFunctions]);


  const contextValue = React.useMemo(
    () => ({
      ...state,
      updateContext,
      filters: filterFunctions
    }),
    [state, updateContext, filterFunctions]
  );

  return (
    <DataTableContext.Provider value={contextValue}>
      <Loader isLoading={state.isLoading} />
      <div className="w-full flex flex-col h-full">
        {children}
      </div>
    </DataTableContext.Provider>
  );
};

interface DataTableContentProps<T extends object> {
  children?: React.ReactNode;
  data: PaginatedResponse<T> | null;
  onFilter?: (activeFilters: Record<string, { operator: string; value: string }[]>) => void;
  skeletonRowCount?: number;
}

DataTable.Content = function DataTableContent<T extends object>({
  children,
  data,
  onFilter,
  skeletonRowCount = 25,
}: DataTableContentProps<T>) {
  const [sorting, setSorting] = React.useState<SortingState>([]);
  const [expanded, setExpanded] = React.useState({});
  const { updateContext, isLoading: contextIsLoading, filters } = useDataTable<T>();

  React.useEffect(() => {
    updateContext({
      isLoading: true
    });

    // When component unmounts, reset loading state
    return () => {
      updateContext({
        isLoading: false
      });
    };
  }, [updateContext]);

  const isLoading = contextIsLoading || !data || !data?.metadata;

  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>([]);
  const [columnVisibility, setColumnVisibility] = React.useState<VisibilityState>({});

  const tableData = React.useMemo(
    () => (isLoading ? Array(50).fill({}) : data?.data || []),
    [isLoading, data]
  );
  
  const loadingColumns: ColumnDef<ApiData<T>>[] = React.useMemo(() =>
    Array(4).fill(null).map((_, index) => ({
      id: `loading-${index}`,
      header: () => <Skeleton className="h-4 w-20" />,
      cell: () => <Skeleton className="h-4 w-80" />,
    })), []
  );

  const [columns, setColumns] = React.useState<ColumnDef<ApiData<T>>[]>(
    isLoading ? loadingColumns : [] // fallback
  );
  
  const table = useReactTable({
    data: tableData,
    columns,
    onColumnFiltersChange: setColumnFilters,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getSortedRowModel: getSortedRowModel(),
    onColumnVisibilityChange: setColumnVisibility,
    onSortingChange: setSorting,
    getExpandedRowModel: getExpandedRowModel(),
    getRowCanExpand: (row) => {
      const attributes = row.original?.attributes;
      if (!attributes) return false;
      
      const expandableMetadata = data?.metadata?.expandable || {};
      const expandableFields = Object.keys(expandableMetadata);
      
      if (expandableFields.length === 0) return false;
      
      return true
    },
    state: {
      columnFilters,
      columnVisibility,
      sorting,
      expanded,
    },
    onExpandedChange: setExpanded,
  });
  
  React.useEffect(() => {
    if (isLoading || !data?.metadata?.columns) return;
  
    const hasExpandableRows = table.getRowModel().rows.some(row => row.getCanExpand());
  
    const newColumns = createColumns<T>(
      data.metadata.columns,
      filters.applyFilter,
      filters.getFilterValue,
      hasExpandableRows
    );

    setColumns(newColumns);
  }, [isLoading, data?.metadata?.columns, filters, table]);
  
  React.useEffect(() => {
    updateContext({
      data,
      isLoading: !data || !data?.metadata,
      columns,
      table,
      onFilter,
      skeletonRowCount
    });
  }, [data, columns, table, updateContext, onFilter, skeletonRowCount, expanded]);

  return (
    <div className="flex-grow overflow-auto">
      <div className="max-h-full flex flex-col flex-auto min-h-0 max-w-full px-6">
        {children}
      </div>
    </div>
  );
};

interface DataTableToolbarProps {
  onSearch?: ((searchTerm: string) => void) | null;
}

DataTable.Toolbar = function DataTableToolbar<TDto extends BaseDto>({
  onSearch,
}: DataTableToolbarProps = {}) {
  const { table, onSearch: contextOnSearch, isLoading, filters } = useDataTable<TDto>();

  const searchHandler = onSearch ?? contextOnSearch;

  const skeletonTable = {
    getAllColumns: () =>
      Array(4)
        .fill(0)
        .map((_, i) => ({
          id: `skeleton-column-${i}`,
          getCanHide: () => true,
          getToggleVisibilityHandler: () => () => { },
          toggleVisibility: () => { },
          getIsVisible: () => true,
        })),
    getColumn: () => null,
  };

  const tableInstance =
    isLoading || !table || typeof table.getHeaderGroups !== 'function'
      ? (skeletonTable as unknown as NonNullable<typeof table>)
      : table;

  const commandList = filters.commands ?? [];

  return (
    <Suspense
      fallback={
        <TableToolbar
          table={skeletonTable as unknown as NonNullable<typeof table>}
          commands={[]}
          onRemoveCommand={() => { }}
          onClearCommands={() => { }}
          onOperatorChange={() => { }}
          onValueChange={() => { }}
        />
      }
    >
      <TableToolbar
        table={tableInstance}
        commands={commandList}
        onRemoveCommand={filters.removeCommand}
        onClearCommands={filters.clearAllFilters}
        onOperatorChange={filters.changeOperator}
        onValueChange={filters.updateFilterValue}
      >
        <TableToolbar.Left>
          <TableToolbar.Search
            onInputChange={searchHandler === null ? undefined : searchHandler}
          />
          <TableToolbar.Commands />
        </TableToolbar.Left>
        <TableToolbar.Right>
          <TableToolbar.Visibility />
        </TableToolbar.Right>
      </TableToolbar>
    </Suspense>
  );
};

interface DataTableContainerProps {
  children?: React.ReactNode;
}

DataTable.Container = function DataTableContainer({
  children,
}: DataTableContainerProps) {
  return <Table className="flex flex-col">{children}</Table>;
};

DataTable.Header = function DataTableHeader() {
  const { table, isLoading } = useDataTable();

  // Show skeleton header if we're loading or don't have a valid table instance
  if (isLoading || !table || typeof table.getHeaderGroups !== 'function') {
    return (
      <TableHeader>
        <TableRow className="flex">
          {Array(4).fill(0).map((_, index) => (
            <TableHead className="flex-1 flex items-center" key={`skeleton-header-${index}`}>
              <Skeleton className="h-4 w-20" />
            </TableHead>
          ))}
        </TableRow>
      </TableHeader>
    );
  }

  return (
    <TableHeader>
      {table.getHeaderGroups().map((headerGroup) => (
        <TableRow className="flex" key={headerGroup.id}>
          {headerGroup.headers.map((header) => (
            <TableHead
              key={header.id}
              className={`${header.column.id === 'expand' ? 'w-10 flex-none' : 'flex-1'}`}
            >
              {header.isPlaceholder
                ? null
                : flexRender(
                  header.column.columnDef.header,
                  header.getContext()
                )}
            </TableHead>
          ))}
        </TableRow>
      ))}
    </TableHeader>
  );
};

DataTable.Body = function DataTableBody() {
  const { table, columns, isLoading, skeletonRowCount } = useDataTable();

  if (isLoading || !table || typeof table.getRowModel !== 'function') {
    return (
      <TableBody className="overflow-y-auto mx-auto w-full">
        {Array(skeletonRowCount).fill(0).map((_, rowIndex) => (
          <TableRow className="flex" key={`skeleton-row-${rowIndex}`}>
            {Array(4).fill(0).map((_, cellIndex) => (
              <TableCell className="flex-1" key={`skeleton-cell-${rowIndex}-${cellIndex}`}>
                <Skeleton className="h-4 w-full" />
              </TableCell>
            ))}
          </TableRow>
        ))}
      </TableBody>
    );
  }

  return (
    <TableBody className="overflow-y-auto mx-auto w-full">
      {table.getRowModel().rows?.length ? (
        table.getRowModel().rows.map((row) => (
          <React.Fragment key={row.id}>
            <DataTable.Row className="flex" row={row} />

            {/* TODO: this should accept children, so that the caller can choose what expand component to use */}
            {row.getIsExpanded() && (
              <DataTable.ExpandedContent row={row} />
            )}
          </React.Fragment>
        ))
      ) : (
        <TableRow className="flex">
          <TableCell
            colSpan={columns.length}
            align="center"
            className="h-24 text-center w-full flex justify-center items-center flex-wrap"
          >
            GLOBAL_NO_DATA
          </TableCell>
        </TableRow>
      )}
    </TableBody>
  );
};

interface DataTablePaginationProps {
  onPaginate?: (link?: Link) => void;
}

DataTable.Pagination = function DataTablePagination({
  onPaginate,
}: DataTablePaginationProps = {}) {
  const { data, onPaginate: contextOnPaginate, isLoading } = useDataTable();

  const paginateHandler = onPaginate || contextOnPaginate;

  // needs to be refined
  if (isLoading || !data || !data.links) {
    return (
      <div className="sticky bottom-0 w-full bg-background border-t">
        <div className="flex items-center justify-between p-4">
          <div className="flex space-x-2">
            <Skeleton className="h-8 w-8 rounded" />
            <Skeleton className="h-8 w-8 rounded" />
          </div>
        </div>
      </div>
    );
  }

  const nextPage = data.links.next ? () => paginateHandler(data.links.next) : undefined;
  const prevPage = data.links.prev ? () => paginateHandler(data.links.prev) : undefined;

  return (
    <div className="sticky bottom-0 w-full bg-background border-t">
      <Pagination
        nextPage={nextPage}
        prevPage={prevPage}
      />
    </div>
  );
};

interface DataTableRowProps {
  row: any;
  children?: React.ReactNode;
}

DataTable.Row = function DataTableRow({
  row,
  children,
  ...props
}: DataTableRowProps) {
  return (
    <TableRow
      className="flex"
      key={row.id}
      data-state={row.getIsSelected() && "selected"}
      {...props}
    >
      {children ?? row.getVisibleCells().map((cell: any) => (
        <TableCell
          key={cell.id}
          className={`${cell.column.id === 'expand' ? 'w-14 flex-none' : 'flex-1'}`}
        >
          {flexRender(cell.column.columnDef.cell, cell.getContext())}
        </TableCell>
      ))}
    </TableRow>
  );
};

interface DataTableCellProps {
  cell: any; // FIXME: no any's!
  children?: React.ReactNode;
}

DataTable.Cell = function DataTableCell({
  cell: any, // FIXME: no any's!
  children,
}: DataTableCellProps) {
  return (
    <TableCell className="flex-1" key={cell.id}>
      {children ?? flexRender(cell.column.columnDef.cell, cell.getContext())}
    </TableCell>
  );
};

interface ExpandedContentProps {
  row: any; // FIXME: no any's!
  children?: React.ReactNode;
}

DataTable.ExpandedContent = function ExpandedContent({
  row,
  children
} : ExpandedContentProps) {
  // Get the metadata from the context instead of the row
  const { data } = useDataTable();
  const expandableMetadata = data?.metadata?.expandable || {};

  const key = Object.keys(expandableMetadata)[0];
  const value = row.original?.attributes?.[key];
  
  // Format the nested data for the DataTable
  const nestedData = {
    data: Array.isArray(value) ? value.map((item, index) => ({
      type: item.type || key,
      id: item.id || index,
      attributes: item.attributes || item,
      links: { self: "#" }
    })) : [],
    metadata:  expandableMetadata[key] || [],
    links: {
      self: {
        href: "#",
        method: "GET",
        rel: "self",
      }
    },
  };

  console.log("Nested Data:", nestedData);
  
  return (
    <div className="pl-14 pr-4 py-4 bg-muted/20">
      {children || (
        <div className="bg-white border rounded-lg shadow-sm">
          <DataTable>
            <DataTable.Content data={nestedData}>
              <DataTable.Toolbar />
              <DataTable.Container>
                <DataTable.Header />
                <DataTable.Body />
              </DataTable.Container>
            </DataTable.Content>
            <DataTable.Pagination />
          </DataTable>
        </div>
      )}
    </div>
  );
};

export { DataTable, useDataTable };