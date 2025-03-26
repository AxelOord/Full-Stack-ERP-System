import { Column } from "@tanstack/react-table"
import { ArrowDown, ArrowUp, EyeOff, Filter, ListFilter } from "lucide-react"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
  DropdownMenuSub,
  DropdownMenuSubTrigger,
  DropdownMenuSubContent,
  DropdownMenuPortal,
} from "@/components/ui/dropdown-menu"
import { Input } from "@/components/ui/input"
import { useEffect, useState, createContext, useContext } from "react"

// Define filter operators that can be used
export const FILTER_OPERATORS = {
  CONTAINS: "contains",
  EQUALS: "eq",
  BEGINS_WITH: "startswith",
  ENDS_WITH: "endsWith",
} as const

export const DEFAULT_OPERATOR = FILTER_OPERATORS.CONTAINS

export type FilterOperator = typeof FILTER_OPERATORS[keyof typeof FILTER_OPERATORS]

interface ColumnHeaderContextValue<TData, TValue> {
  column: Column<TData, TValue>
  title: string
  field: string
  open: boolean
  setOpen: (value: boolean) => void
  defaultOperator: FilterOperator
  onFilterApply?: (field: string, value: string, operator: FilterOperator) => void
  getFilterValue?: (field: string, operator: FilterOperator) => string | null
  currentFilterValue: string
  currentOperator: FilterOperator
}

// should fix this, just need to figure out how to type this properly :)
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const ColumnHeaderContext = createContext<ColumnHeaderContextValue<any, any> | undefined>(undefined)

function useColumnHeaderContext<TData, TValue>() {
  const context = useContext(ColumnHeaderContext) as ColumnHeaderContextValue<TData, TValue> | undefined
  if (!context) {
    throw new Error("Column header components must be used within a DataTableColumnHeader")
  }
  return context
}

interface DataTableColumnHeaderProps<TData, TValue>
  extends React.HTMLAttributes<HTMLDivElement> {
  column: Column<TData, TValue>
  title: string
  field: string
  defaultOperator?: FilterOperator
  onFilterApply?: (field: string, value: string, operator: FilterOperator) => void
  getFilterValue?: (field: string, operator: FilterOperator) => string | null
}

export function DataTableColumnHeader<TData, TValue>({
  column,
  title,
  field,
  className,
  defaultOperator = DEFAULT_OPERATOR,
  onFilterApply,
  getFilterValue,
  children,
}: React.PropsWithChildren<DataTableColumnHeaderProps<TData, TValue>>) {
  const [open, setOpen] = useState(false)
  
  const getCurrentFilterValue = () => {
    if (getFilterValue) {
      for (const op of Object.values(FILTER_OPERATORS)) {
        const value = getFilterValue(field, op as FilterOperator)
        if (value) {
          return { operator: op as FilterOperator, value }
        }
      }
    }
    return { operator: defaultOperator, value: "" }
  }

  const { operator: currentOperator, value: currentFilterValue } = getCurrentFilterValue()

  const contextValue = {
    column,
    title,
    field,
    open,
    setOpen,
    defaultOperator,
    onFilterApply,
    getFilterValue,
    currentFilterValue,
    currentOperator,
  }

  return (
    <ColumnHeaderContext.Provider value={contextValue}>
      <div className={cn("flex items-center space-x-2 basis-full justify-between", className)}>
        <DropdownMenu open={open} onOpenChange={setOpen}>
          <DropdownMenuTrigger asChild>
            <Button
              variant="ghost"
              size="sm"
              className="-ml-3 h-8 data-[state=open]:bg-accent w-full justify-between"
            >
              <span>{title}</span>
              <ListFilter className="h-3.5 w-3.5 text-muted-foreground/70" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-48">
            {children}
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </ColumnHeaderContext.Provider>
  )
}

DataTableColumnHeader.Sort = function Sort<TData, TValue>() {
  const { column } = useColumnHeaderContext<TData, TValue>()
  
  if (!column.getCanSort()) {
    return null
  }

  return (
    <>
      <DropdownMenuItem onClick={() => column.toggleSorting(false)}>
        <ArrowUp className="mr-2 h-3.5 w-3.5 text-muted-foreground/70" />
        Asc
      </DropdownMenuItem>
      <DropdownMenuItem onClick={() => column.toggleSorting(true)}>
        <ArrowDown className="mr-2 h-3.5 w-3.5 text-muted-foreground/70" />
        Desc
      </DropdownMenuItem>
      <DropdownMenuSeparator />
    </>
  )
}

interface FilterProps {
  operators?: Record<string, FilterOperator>
}

DataTableColumnHeader.Filter = function ColumnFilter<TData, TValue>({ 
  operators = FILTER_OPERATORS 
}: FilterProps) {
  const { 
    title, 
    field, 
    onFilterApply, 
    currentOperator, 
    currentFilterValue, 
    setOpen 
  } = useColumnHeaderContext<TData, TValue>()
  
  const [filterValue, setFilterValue] = useState(currentFilterValue)
  const [operator, setOperator] = useState<FilterOperator>(currentOperator)

  useEffect(() => {
    setOperator(currentOperator)
    setFilterValue(currentFilterValue)
  }, [currentOperator, currentFilterValue])

  const handleFilterSubmit = () => {
    if (filterValue.trim() && onFilterApply) {
      onFilterApply(field, filterValue.trim(), operator)
      setOpen(false)
    }
  }

  return (
    <DropdownMenuSub>
      <DropdownMenuSubTrigger>
        <Filter className="mr-2 h-3.5 w-3.5 text-muted-foreground/70" />
        Filter
      </DropdownMenuSubTrigger>
      <DropdownMenuPortal>
        <DropdownMenuSubContent className="p-3 min-w-[320px] shadow-md rounded-md">
          <div className="flex flex-col gap-2">
            <div className="text-xs text-muted-foreground font-normal mb-1">
              {title}
            </div>

            <div className="flex items-center gap-1">
              <select
                value={operator}
                onChange={(e) => setOperator(e.target.value as FilterOperator)}
                className="h-7 rounded-md border border-input bg-background px-2 text-xs"
                style={{ minWidth: "90px" }}
              >
                {Object.entries(operators).map(([key, value]) => (
                  <option key={value} value={value}>
                    {key.toLowerCase().replace('_', ' ')}
                  </option>
                ))}
              </select>

              <Input
                placeholder="Value..."
                value={filterValue}
                onChange={(e) => setFilterValue(e.target.value)}
                className="h-7 text-xs w-32"
                onKeyDown={(e) => {
                  if (e.key === 'Enter' && filterValue.trim()) {
                    handleFilterSubmit()
                  }
                }}
              />

              <Button
                size="sm"
                className="h-7 text-xs"
                onClick={handleFilterSubmit}
              >
                Apply
              </Button>
            </div>
          </div>
        </DropdownMenuSubContent>
      </DropdownMenuPortal>
    </DropdownMenuSub>
  )
}

DataTableColumnHeader.Visibility = function Visibility<TData, TValue>() {
  const { column } = useColumnHeaderContext<TData, TValue>()
  
  return (
    <DropdownMenuItem onClick={() => column.toggleVisibility(false)}>
      <EyeOff className="mr-2 h-3.5 w-3.5 text-muted-foreground/70" />
      Hide
    </DropdownMenuItem>
  )
}

DataTableColumnHeader.Separator = function Separator() {
  return <DropdownMenuSeparator />
}