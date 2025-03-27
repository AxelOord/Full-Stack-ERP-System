"use client";

import React, { createContext, useMemo, useCallback } from 'react';
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuItem,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import InputSearch from "@/components/table/input-search";
import { Table } from '@tanstack/react-table';
import { X } from "lucide-react";
import { useRouter, useSearchParams } from 'next/navigation';
import { cn } from "@/lib/utils";
import { BaseDto } from '@/services/models/BaseDto';
import { useDataTableFilters } from "@/hooks/use-data-table-filters";

// Constants
const EXCLUDED_PARAMS = ['page', 'size', 'q'];
const COMMAND_SEPARATOR = ':';

// Types
export interface Command {
  key: string;
  value: string;
  raw: string;
}

export interface CommandsProps {
  commands?: string[];
  onRemoveCommand?: (command: string) => void;
  onClearCommands?: () => void;
  clearButtonText?: string;
}

export interface SearchProps {
  onInputChange?: (searchTerm: string) => void;
}

export interface VisibilityProps {
  buttonText?: string;
}

export interface TableToolbarProps<TDto extends BaseDto> extends React.HTMLAttributes<HTMLDivElement> {
  table: Table<TDto>;
  commands?: Command[];
  onRemoveCommand?: (command: string) => void;
  onClearCommands?: () => void;
  onOperatorChange?: (field: string, oldOperator: string, newOperator: string, value: string) => void;
  onValueChange?: (field: string, operator: string, newValue: string) => void;
}

// Utilities
function parseCommand(commandString: string): Command {
  const [key, value] = commandString.split(COMMAND_SEPARATOR);
  return {
    key,
    value,
    raw: commandString
  };
}

// Context
interface TableToolbarContextValue<TDto extends BaseDto> {
  table: Table<TDto>;
  commands: Command[];
  onRemoveCommand: (command: string) => void;
  onClearCommands: () => void;
  onOperatorChange?: (field: string, oldOperator: string, newOperator: string, value: string) => void;
  onValueChange?: (field: string, operator: string, newValue: string) => void;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const TableToolbarContext = createContext<any>(undefined);

function useTableToolbarContext<TDto extends BaseDto>(): TableToolbarContextValue<TDto> {
  const context = React.useContext(TableToolbarContext);
  if (!context) {
    throw new Error("useDataTable must be used within a DataTable");
  }
  return context as TableToolbarContextValue<TDto>;
}

export function TableToolbar<TDto extends BaseDto>({
  table,
  className,
  children,
  commands: explicitCommands = [],
  onRemoveCommand,
  onClearCommands,
  onOperatorChange,
  onValueChange,
  ...props
}: React.PropsWithChildren<TableToolbarProps<TDto>>) {
  const parsedCommands: Command[] = useMemo(
    () => explicitCommands.map(parseCommand),
    [explicitCommands]
  );

  const contextValue = {
    table,
    commands: parsedCommands,
    onRemoveCommand: onRemoveCommand ?? (() => {}),
    onClearCommands: onClearCommands ?? (() => {}),
    onOperatorChange,
    onValueChange
  };

  return (
    <TableToolbarContext.Provider value={contextValue}>
      <div
        className={cn(
          "flex flex-col sm:flex-row items-start sm:items-center py-4 gap-2",
          className
        )}
        {...props}
      >
        {children}
      </div>
    </TableToolbarContext.Provider>
  );
}

// Search Component
TableToolbar.Search = function Search({
  onInputChange
}: SearchProps) {
  return <InputSearch onInputChange={onInputChange} />;
};

// Commands Component
TableToolbar.Commands = function Commands({
  commands: explicitCommands,
  onRemoveCommand: explicitRemoveHandler,
  onClearCommands: explicitClearHandler,
  clearButtonText = "GLOBAL_CLEAR"
}: CommandsProps) {
  const { commands: contextCommands, onRemoveCommand: contextRemoveHandler, onClearCommands: contextClearHandler } = useTableToolbarContext();

  // Use explicit handlers if provided, otherwise use context handlers
  const commands = explicitCommands ? explicitCommands.map(parseCommand) : contextCommands;
  const handleRemoveCommand = explicitRemoveHandler || contextRemoveHandler;
  const handleClearCommands = explicitClearHandler || contextClearHandler;

  if (commands.length === 0) return null;

  console.log('rendering commands', commands);

  return (
    <div className="flex gap-2 flex-wrap">
      {commands.map((command) => (
        <TableToolbar.CommandTag
          key={command.raw}
          command={command}
          onRemove={handleRemoveCommand}
        />
      ))}

      <Button
        variant="outline"
        size="sm"
        className="transition group h-6 text-xs items-center rounded-sm"
        onClick={handleClearCommands}
      >
        {clearButtonText}
      </Button>
    </div>
  );
};

// Command Tag Component
interface CommandTagProps {
  command: Command;
  onRemove: (cmd: string) => void;
}

TableToolbar.CommandTag = function CommandTag({
  command,
  onRemove
}: CommandTagProps) {
  const { onOperatorChange, onValueChange } = useTableToolbarContext();

  // Extract operator from command if it exists (e.g., supplierName[contains]:012)
  const [key, value] = command.raw.split(':');
  const keyParts = key.match(/^(.*?)\[(.*?)\]$/);

  const fieldName = keyParts ? keyParts[1] : command.key;
  const operator = keyParts ? keyParts[2] : "contains";
  const displayValue = value || command.value;

  const [inputValue, setInputValue] = React.useState(displayValue);
  const [isEditing, setIsEditing] = React.useState(false);
  const inputRef = React.useRef<HTMLInputElement>(null);

  // Handle operator changes by delegating to the context handler
  const handleOperatorChange = (newOperator: string) => {
    if (onOperatorChange) {
      onOperatorChange(fieldName, operator, newOperator, displayValue);
    }
  };

  // Handle value changes by delegating to the context handler
  const handleValueChange = (newValue: string) => {
    if (newValue.trim() === '') return;

    if (onValueChange) {
      onValueChange(fieldName, operator, newValue);
    }

    setIsEditing(false);
  };

  // Focus input when editing starts
  React.useEffect(() => {
    if (isEditing && inputRef.current) {
      inputRef.current.focus();
    }
  }, [isEditing]);

  return (
    <div className="flex gap-[1px] items-center text-xs">
      <div className="flex gap-1.5 shrink-0 rounded-l bg-muted px-1.5 py-1 items-center">
        <span className="font-medium">{fieldName}</span>
      </div>
      <DropdownMenu>
        <DropdownMenuTrigger className="bg-muted hover:bg-muted/50 px-1.5 py-1 text-muted-foreground hover:text-primary transition shrink-0">
          {operator}
        </DropdownMenuTrigger>
        <DropdownMenuContent align="start" className="w-fit min-w-fit">
          <DropdownMenuItem onClick={() => handleOperatorChange('contains')}>
            contains
          </DropdownMenuItem>
          <DropdownMenuItem onClick={() => handleOperatorChange('eq')}>
            is
          </DropdownMenuItem>
          <DropdownMenuItem onClick={() => handleOperatorChange('startsWith')}>
            begins with
          </DropdownMenuItem>
          <DropdownMenuItem onClick={() => handleOperatorChange('endsWith')}>
            ends with
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
      {isEditing ? (
        <input
          ref={inputRef}
          type="text"
          className="bg-muted px-1.5 py-1 text-xs outline-none focus:ring-1 focus:ring-primary min-w-0 w-auto"
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          onBlur={() => handleValueChange(inputValue)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              handleValueChange(inputValue);
            } else if (e.key === 'Escape') {
              setInputValue(displayValue);
              setIsEditing(false);
            }
          }}
          size={inputValue.length}
        />
      ) : (
        <div
          className="flex gap-1.5 shrink-0 bg-muted px-1.5 py-1 items-center cursor-pointer"
          onClick={() => setIsEditing(true)}
        >
          {displayValue}
        </div>
      )}
      <Button
        variant="ghost"
        size="icon"
        onClick={() => onRemove(command.raw)}
        className="bg-muted rounded-l-none rounded-r-sm h-6 w-6 text-muted-foreground hover:text-primary hover:bg-muted/50 transition shrink-0"
      >
        <X className="size-3" />
      </Button>
    </div>
  );
};

// Column Visibility Component
TableToolbar.Visibility = function Visibility({
  buttonText = "GLOBAL_VIEW"
}: VisibilityProps) {
  const { table } = useTableToolbarContext();

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="outline" className="ml-auto">
          {buttonText}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        {table
          .getAllColumns()
          .filter((column) => column.getCanHide())
          .map((column) => (
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
          ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

// Flexible container for left side content
TableToolbar.Left = function Left({
  className,
  children,
  ...props
}: React.HTMLAttributes<HTMLDivElement>) {
  return (
    <div className={cn("flex-1 flex items-center gap-2 w-full sm:w-auto flex-wrap", className)} {...props}>
      {children}
    </div>
  );
};

// Flexible container for right side content
TableToolbar.Right = function Right({
  className,
  children,
  ...props
}: React.HTMLAttributes<HTMLDivElement>) {
  return (
    <div className={cn("flex items-center gap-2", className)} {...props}>
      {children}
    </div>
  );
};

export default TableToolbar;