import { useRouter, useSearchParams } from 'next/navigation';
import { useCallback, useMemo, useState } from 'react';
import { FILTER_OPERATORS, FilterOperator } from '@/components/table/data-table-column-header';

type FilterValue = { operator: string; value: string };
type ActiveFilters = Record<string, FilterValue[]>;

export function useDataTableFilters(
  onFiltersChange?: (activeFilters: ActiveFilters) => void
) {
  const router = useRouter();
  const searchParams = useSearchParams();

  const [commands, setCommands] = useState<string[]>([]);
  const [activeFilters, setActiveFilters] = useState<ActiveFilters>({});

  const parseCommand = useCallback((commandString: string) => {
    const [key, value] = commandString.split(':');
    return { key, value, raw: commandString };
  }, []);

  const extractCommandsFromParams = useCallback((params: URLSearchParams): string[] => {
    const list: string[] = [];
    for (const [key, value] of params.entries()) {
      list.push(`${key}:${value}`);
    }
    return list;
  }, []);  

  const extractActiveFiltersFromParams = useCallback((params: URLSearchParams): ActiveFilters => {
    const filters: ActiveFilters = {};
    for (const [key, value] of params.entries()) {
      const match = key.match(/^([^\[]+)\[([^\]]+)\]$/);
      if (!match) continue;
      const [, field, operator] = match;
      if (!filters[field]) filters[field] = [];
      filters[field].push({ operator, value });
    }
    return filters;
  }, []);

  const syncStateFromParams = useCallback((params: URLSearchParams) => {
    setCommands(extractCommandsFromParams(params));
    const filters = extractActiveFiltersFromParams(params);
    setActiveFilters(filters);
    onFiltersChange?.(filters);
  }, [extractCommandsFromParams, extractActiveFiltersFromParams, onFiltersChange]);

  const withUpdatedParams = useCallback(
    (modifier: (params: URLSearchParams) => void) => {
      const params = new URLSearchParams(searchParams.toString());
      modifier(params);
      router.push(`?${params.toString()}`);
      syncStateFromParams(params); // <-- update internal state immediately
    },
    [router, searchParams, syncStateFromParams]
  );

  const applyFilter = useCallback(
    (field: string, value: string, operator: FilterOperator) => {
      withUpdatedParams((params) => {
        Object.values(FILTER_OPERATORS).forEach((op) => {
          params.delete(`${field}[${op}]`);
        });
        params.set(`${field}[${operator}]`, value);
      });
    },
    [withUpdatedParams]
  );

  const getFilterValue = useCallback(
    (field: string, operator: FilterOperator): string | null => {
      return activeFilters[field]?.find(f => f.operator === operator)?.value ?? null;
    },
    [activeFilters]
  );

  const changeOperator = useCallback(
    (field: string, oldOp: string, newOp: string, value: string) => {
      withUpdatedParams((params) => {
        params.delete(`${field}[${oldOp}]`);
        params.set(`${field}[${newOp}]`, value);
      });
    },
    [withUpdatedParams]
  );

  const updateFilterValue = useCallback(
    (field: string, operator: string, newValue: string) => {
      withUpdatedParams((params) => {
        params.set(`${field}[${operator}]`, newValue);
      });
    },
    [withUpdatedParams]
  );

  const removeCommand = useCallback(
    (command: string) => {
      const { key } = parseCommand(command);
      withUpdatedParams((params) => {
        params.delete(key);
      });
    },
    [parseCommand, withUpdatedParams]
  );

  const clearAllFilters = useCallback(() => {
    withUpdatedParams((params) => {
      const exclude = ['page', 'size', 'q'];
      Array.from(params.keys()).forEach((key) => {
        if (!exclude.includes(key)) params.delete(key);
      });
    });
  }, [withUpdatedParams]);

  return useMemo(
    () => ({
      applyFilter,
      getFilterValue,
      changeOperator,
      updateFilterValue,
      removeCommand,
      clearAllFilters,
      getActiveFilters: () => activeFilters,
      parseCommand,
      searchParams,
      commands,
    }),
    [
      applyFilter,
      getFilterValue,
      changeOperator,
      updateFilterValue,
      removeCommand,
      clearAllFilters,
      activeFilters,
      parseCommand,
      searchParams,
      commands,
    ]
  );
}
