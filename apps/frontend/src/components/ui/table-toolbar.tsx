import React from 'react';
import { Button } from "./button"
import {
    DropdownMenu,
    DropdownMenuCheckboxItem,
    DropdownMenuContent,
    DropdownMenuTrigger,
  } from "@/components/ui/dropdown-menu"
import InputSearch from "./input-search"
import { Table } from '@tanstack/react-table';

interface TableToolbarProps {
    table: Table<any>;
    onInputChange: (searchTerm: string) => void;
  }

const TableToolbar = ({ table, onInputChange } : TableToolbarProps) => {
  return (
    <div className="flex items-center py-4">
      <InputSearch onInputChange={(value) => onInputChange(value)} />
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="outline" className="ml-auto">
            GLOBAL_VIEW
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end">
          {table
            .getAllColumns()
            .filter((column) => column.getCanHide())
            .map((column) => {
              return (
                <DropdownMenuCheckboxItem
                  key={column.id}
                  className="capitalize"
                  checked={column.getIsVisible()}
                  onCheckedChange={(value) =>
                    column.toggleVisibility(!!value)
                  }
                >
                  {column.id}
                </DropdownMenuCheckboxItem>
              );
            })}
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  );
};

export default TableToolbar;