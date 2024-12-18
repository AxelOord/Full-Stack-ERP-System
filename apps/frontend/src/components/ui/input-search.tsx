"use client";

import { Input } from "@/components/ui/input";
import { CircleX } from "lucide-react";
import { useRef, useState } from "react";
import { Search } from "lucide-react";

interface InputSearchProps {
  onInputChange?: (value: string) => void;
}

export default function InputSearch({ onInputChange }: InputSearchProps) {
  const [inputValue, setInputValue] = useState("");
  const inputRef = useRef<HTMLInputElement>(null);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setInputValue(value);
    if (onInputChange) {
      onInputChange(value);
    }
  };

  const handleClearInput = () => {
    setInputValue("");
    if (inputRef.current) {
      inputRef.current.focus();
    }
    if (onInputChange) {
      onInputChange("");
    }
  };

  return (
    <div className="space-y-2">
      <div className="relative">
        <Input
          id="input-search"
          ref={inputRef}
          className="peer pe-9 ps-9"
          placeholder="Zoek artikel..."
          type="text"
          value={inputValue}
          onChange={handleInputChange}
        />
        <div className="pointer-events-none absolute inset-y-0 start-0 flex items-center justify-center ps-3 text-muted-foreground/80 peer-disabled:opacity-50 ">
          <Search size={16} strokeWidth={2} />
        </div>
        {inputValue && (
          <button
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
