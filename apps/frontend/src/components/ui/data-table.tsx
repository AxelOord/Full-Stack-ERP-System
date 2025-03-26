"use client"

import {
  ColumnDef,
  ColumnFiltersState,
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  useReactTable,
  VisibilityState,
} from "@tanstack/react-table"

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import React, { useMemo } from "react"
import { createColumns } from "./columns"
import { ApiData, Link, PaginatedResponse } from "@/services";
import { Skeleton } from "./skeleton"
import Loader from "../shared/loader"
import TableToolbar from "./table-toolbar"
import Pagination from "./pagination"

interface DataTableContextValue<T extends object> {
  table: ReturnType<typeof useReactTable<any>> | null;
  data: PaginatedResponse<T> | null;
  isLoading: boolean;
  columns: ColumnDef<ApiData<T>>[];
  onSearch: ((searchTerm: string) => void) | null;
  onFilter?: (column: string, value: string) => void;
  onPaginate: (link?: Link) => void;
  updateContext: (newState: Partial<DataTableContextState<T>>) => void;
}

interface DataTableContextState<T extends object> {
  table: ReturnType<typeof useReactTable<any>> | null;
  data: PaginatedResponse<T> | null;
  isLoading: boolean;
  columns: ColumnDef<ApiData<T>>[];
  onSearch: ((searchTerm: string) => void) | null;
  onFilter?: (column: string, value: string) => void;
  onPaginate: (link?: Link) => void;
}

const DataTableContext = React.createContext<DataTableContextValue<any> | undefined>(undefined);

function useDataTable<T extends object>() {
  const context = React.useContext(DataTableContext);
  if (!context) {
    throw new Error("useDataTable must be used within a DataTable");
  }
  return context as DataTableContextValue<T>;
}

interface DataTableProps {
  children?: React.ReactNode;
}

const DataTable = ({ children }: DataTableProps) => {
  const [state, setState] = React.useState<DataTableContextState<any>>({
    table: null,
    data: null,
    isLoading: true,
    columns: [],
    onSearch: null,
    onPaginate: () => {},
  });

  const updateContext = React.useCallback((newState: Partial<DataTableContextState<any>>) => {
    setState(prevState => ({ ...prevState, ...newState }));
  }, []);

  const contextValue = React.useMemo(
    () => ({
      ...state,
      updateContext,
    }),
    [state, updateContext]
  );

  return (
    <DataTableContext.Provider value={contextValue}>
      <div className="w-full flex flex-col h-full">
        {children}
      </div>
    </DataTableContext.Provider>
  );
};

interface DataTableContentProps<T extends object> {
  children?: React.ReactNode;
  data: PaginatedResponse<T> | null;
  onFilter?: (column: string, value: string) => void;
}

DataTable.Content = function DataTableContent<T extends object>({
  children,
  data,
  onFilter,
}: DataTableContentProps<T>) {
  const { updateContext, isLoading: contextIsLoading } = useDataTable<T>();

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

  const columnsMemo = useMemo(
    () => (
      isLoading 
        ? loadingColumns 
        : createColumns<T>(data?.metadata?.columns || [], onFilter)
    ),
    [isLoading, data?.metadata?.columns, loadingColumns, onFilter]
  );

  const table = useReactTable({
    data: tableData,
    columns: columnsMemo,
    onColumnFiltersChange: setColumnFilters,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    onColumnVisibilityChange: setColumnVisibility,
    state: {
      columnFilters,
      columnVisibility,
    },
  });

  const contextRef = React.useRef({
    data: null as PaginatedResponse<T> | null,
    table: null as ReturnType<typeof useReactTable<any>> | null
  });
  
  React.useEffect(() => {
    if (
      data !== contextRef.current.data ||
      table !== contextRef.current.table
    ) {
      contextRef.current.data = data;
      contextRef.current.table = table;
      
      updateContext({
        data,
        isLoading: !data || !data?.metadata,
        columns: columnsMemo,
        table,
        onFilter
      });
    }
  }, [data, isLoading, columnsMemo, table, updateContext, onFilter]);

  return (
    <div className="flex-grow overflow-auto">
      <div className="max-h-full flex flex-col flex-auto min-h-0 max-w-full px-6">
        {children}
      </div>
    </div>
  );
};

DataTable.Loader = function DataTableLoader() {
  const { isLoading } = useDataTable();
  return <Loader isLoading={isLoading} />;
};

interface DataTableToolbarProps {
  onSearch?: ((searchTerm: string) => void) | null;
}

DataTable.Toolbar = function DataTableToolbar({
  onSearch,
}: DataTableToolbarProps = {}) {
  const { table, onSearch: contextOnSearch, isLoading } = useDataTable();
  
  // Use prop if provided, otherwise fall back to context
  const searchHandler = onSearch !== undefined ? onSearch : contextOnSearch;

  const skeletonTable = {
    getAllColumns: () => Array(4).fill(0).map((_, i) => ({ 
      id: `skeleton-column-${i}`,
      getCanHide: () => true,
      getToggleVisibilityHandler: () => () => {},
      toggleVisibility: () => {},
      getIsVisible: () => true
    })),
    getColumn: () => null,
  };
  
  if (isLoading || !table || typeof table.getHeaderGroups !== 'function') {
    return <TableToolbar 
      table={skeletonTable as any} 
      onInputChange={null} 
    />;
  }

  return <TableToolbar 
    table={table} 
    onInputChange={searchHandler === undefined ? null : searchHandler}
  />;
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
            <TableHead className="flex-1 flex items-center" key={header.id}>
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
  const { table, columns, isLoading } = useDataTable();
  
  if (isLoading || !table || typeof table.getRowModel !== 'function') {
    return (
      <TableBody className="overflow-y-auto mx-auto w-full">
        {Array(50).fill(0).map((_, rowIndex) => (
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
          <TableRow
            className="flex"
            key={row.id}
            data-state={row.getIsSelected() && "selected"}
          >
            {row.getVisibleCells().map((cell) => (
              <TableCell className="flex-1" key={cell.id}>
                {flexRender(cell.column.columnDef.cell, cell.getContext())}
              </TableCell>
            ))}
          </TableRow>
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
  
  // Use prop if provided, otherwise fall back to context
  const paginateHandler = onPaginate || contextOnPaginate;

  if (isLoading || !data || !data.links) {
    return (
      <div className="sticky bottom-0 w-full bg-background border-t">
        <div className="flex items-center justify-between p-4">
          <Skeleton className="h-8 w-20" />
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
}: DataTableRowProps) {
  return (
    <TableRow
      className="flex"
      key={row.id}
      data-state={row.getIsSelected() && "selected"}
    >
      {children ?? row.getVisibleCells().map((cell: any) => (
        <DataTable.Cell key={cell.id} cell={cell} />
      ))}
    </TableRow>
  );
};

interface DataTableCellProps {
  cell: any; // FIXME: no any's!
  children?: React.ReactNode;
}

DataTable.Cell = function DataTableCell({
  cell,
  children,
}: DataTableCellProps) {
  return (
    <TableCell className="flex-1" key={cell.id}>
      {children ?? flexRender(cell.column.columnDef.cell, cell.getContext())}
    </TableCell>
  );
};

export { DataTable, useDataTable };
