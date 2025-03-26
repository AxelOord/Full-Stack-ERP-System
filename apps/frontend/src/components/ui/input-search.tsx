"use client";

import * as React from "react";
import { Input } from "@/components/ui/input";
import { CircleX, Search } from "lucide-react";
import { cn } from "@/lib/utils";

export interface InputSearchProps
  extends Omit<React.InputHTMLAttributes<HTMLInputElement>, 'onChange'> {
  onInputChange?: ((value: string) => void) | null;
  containerClassName?: string;
}

const InputSearch = React.forwardRef<HTMLInputElement, InputSearchProps>(
  ({ onInputChange, className, containerClassName, ...props }, ref) => {
    const [inputValue, setInputValue] = React.useState("");
    const inputRef = React.useRef<HTMLInputElement>(null);
    
    // Use the forwarded ref or internal ref
    const resolvedRef = ref || inputRef;

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const value = e.target.value;
      setInputValue(value);
      if (onInputChange) {
        onInputChange(value);
      }
    };

    const handleClearInput = () => {
      setInputValue("");
      if ('current' in resolvedRef && resolvedRef.current) {
        resolvedRef.current.focus();
      }
      if (onInputChange) {
        onInputChange("");
      }
    };

    return (
      <div className={cn("space-y-2", containerClassName)}>
        <div className="relative">
          <Input
            ref={resolvedRef}
            className={cn("peer pe-9 ps-9", className)}
            placeholder="Zoek artikel..."
            type="text"
            value={inputValue}
            onChange={handleInputChange}
            disabled={!onInputChange}
            {...props}
          />
          <div className="pointer-events-none absolute inset-y-0 start-0 flex items-center justify-center ps-3 text-muted-foreground/80 peer-disabled:opacity-50">
            <Search size={16} strokeWidth={2} />
          </div>
          {inputValue && (
            <button
              type="button"
              className="absolute inset-y-0 end-0 flex h-full w-9 items-center justify-center rounded-e-lg text-muted-foreground/80 outline-offset-2 transition-colors hover:text-foreground focus:z-10 focus-visible:outline focus-visible:outline-2 focus-visible:outline-ring/70 disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50"
              aria-label="Clear input"
              onClick={handleClearInput}
            >
              <CircleX size={16} strokeWidth={2} aria-hidden="true" />
            </button>
          )}
        </div>
      </div>
    );
  }
);

InputSearch.displayName = "InputSearch";

export { InputSearch };
export default InputSearch;