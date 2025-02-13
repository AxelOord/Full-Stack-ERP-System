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
import { ApiData, PaginatedResponse } from "@/services";
import { Skeleton } from "./skeleton"
import Loader from "../shared/loader"
import TableToolbar from "./table-toolbar"
import Pagination from "./pagination"

interface DataTableProps<T> {
  data: PaginatedResponse<T> | null;
  onSearch: ((searchTerm: string) => void ) | null;
  onPaginate: (link?: string | null | undefined) => void;
}

export function DataTable<T extends object>({ data, onSearch, onPaginate }: DataTableProps<T>) {
  const isLoading = !data || !data?.metadata;

  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>(
    []
  )
  const [columnVisibility, setColumnVisibility] =
    React.useState<VisibilityState>({})

  const tableData = React.useMemo(
    () => (isLoading ? Array(50).fill({}) : data.data),
    [isLoading, data]
  );

  const loadingColumns: ColumnDef<ApiData<T>>[] = Array(4).fill(null).map((_, index) => ({
    id: `loading-${index}`,
    header: () => <Skeleton className="h-4 w-20" />,
    cell: () => <Skeleton className="h-4 w-80" />,
  }));

  const columnsMemo = useMemo(
    () => (isLoading ? loadingColumns : createColumns<T>(data?.metadata?.columns || [])),
    [isLoading, data?.metadata?.columns, loadingColumns]
  );

  const table = useReactTable({
    data: tableData || [],
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

  return (
    <>
      <div className="max-h-full flex flex-col flex-auto min-h-0 max-w-full px-6">
        <Loader isLoading={isLoading} />
        <TableToolbar table={table} onInputChange={onSearch} />
        <Table className="flex flex-col">
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
                <TableCell colSpan={columnsMemo.length} align="center" className="h-24 text-center w-full flex justify-center items-center flex-wrap">
                  GLOBAL_NO_DATA
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      <Pagination
          nextPage={data?.links?.next ? () => onPaginate(data.links.next) : undefined}
          prevPage={data?.links?.prev ? () => onPaginate(data.links.prev) : undefined}
        />
    </>
  );
}