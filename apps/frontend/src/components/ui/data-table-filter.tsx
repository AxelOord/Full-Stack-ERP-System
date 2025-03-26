// src/components/ui/data-table-filter.tsx
import React, { useState } from "react";
import { Column } from "@tanstack/react-table";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { ListFilter as Filter} from "lucide-react";

interface DataTableFilterProps<TData, TValue> {
  column: Column<TData, TValue>;
  field: string;
  onFilter: (column: string, value: string) => void;
}

export function DataTableFilter<TData, TValue>({
  column,
  field,
  onFilter,
}: DataTableFilterProps<TData, TValue>) {
  const [value, setValue] = useState<string>("");
  
  const handleFilter = () => {
    onFilter(field, value);
  };
  
  const handleClear = () => {
    setValue("");
    onFilter("", "");
  };

  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button 
          variant="ghost" 
          size="sm" 
          className="-ml-3 h-8"
        >
          <Filter className="h-4 w-4" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-80 p-4">
        <div className="space-y-4">
          <h4 className="font-medium">Filter by {column.id.split('.').pop()}</h4>
          <div className="space-y-2">
            <Input
              placeholder="Enter value..."
              value={value}
              onChange={(e) => setValue(e.target.value)}
              className="h-8"
            />
            <div className="flex items-center justify-end space-x-2">
              <Button variant="outline" size="sm" onClick={handleClear}>
                Clear
              </Button>
              <Button size="sm" onClick={handleFilter}>
                Apply
              </Button>
            </div>
          </div>
        </div>
      </PopoverContent>
    </Popover>
  );
}