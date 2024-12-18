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
import { ApiData, Response } from "@/services";
import { Skeleton } from "./skeleton"
import Loader from "../shared/loader"
import TableToolbar from "./table-toolbar"
import Pagination from "./pagination"

interface DataTableProps<T> {
  result: Response<T> | null;
  hasNextPage: boolean;
  hasPrevPage: boolean;
  onNextPage: () => void;
  onPrevPage: () => void;
  onInputChange: (searchTerm: string) => void;
}


export function DataTable<T extends object>({
  result,
  hasNextPage,
  hasPrevPage,
  onNextPage,
  onPrevPage,
  onInputChange,
}: DataTableProps<T>) {

  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>(
    []
  )
  const [columnVisibility, setColumnVisibility] =
    React.useState<VisibilityState>({})

  const isLoading = !result || !result?.metadata;

  const tableData = React.useMemo(
    () => (isLoading ? Array(50).fill({}) : result.data),
    [isLoading, result]
  );

  const loadingColumns: ColumnDef<ApiData<T>>[] = Array(3).fill(null).map((_, index) => ({
    id: `loading-${index}`,
    header: () => <Skeleton className="h-4 w-20" />,
    cell: () => <Skeleton className="h-4 w-80" />,
  }));

  const columnsMemo = useMemo(
    () => (isLoading ? loadingColumns : createColumns<T>(result?.metadata.columns || [])),
    [isLoading, result?.metadata.columns]
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

  return (
    <>
      <div className="max-h-full flex flex-col flex-auto min-h-0 max-w-full px-6">
        <Loader isLoading={isLoading} />
        <TableToolbar table={table} onInputChange={onInputChange} />
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
          hasPrevPage={hasPrevPage}
          hasNextPage={hasNextPage}
          onPrevPage={onPrevPage}
          onNextPage={onNextPage}
        />
    </>
  );
}