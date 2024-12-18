import React from "react";
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog";

interface LoaderProps {
  isLoading: boolean;
}

const Loader: React.FC<LoaderProps> = ({ isLoading }) => {
  if (!isLoading) return null;

  return (
    <Dialog open={isLoading}>
      <DialogContent className="bg-white rounded-lg py-8 px-24 shadow-lg border-gray [&>button]:hidden w-auto">
        <DialogHeader>
          <div className="flex items-center justify-center space-x-2 py-8">
            <div
              className="w-5 h-5 bg-turquoise animate-wave"
              style={{ animationDelay: "0ms" }}
            ></div>
            <div
              className="w-5 h-5 bg-orange animate-wave"
              style={{ animationDelay: "100ms" }}
            ></div>
            <div
              className="w-5 h-5 bg-blue animate-wave"
              style={{ animationDelay: "200ms" }}
            ></div>
            <div
              className="w-5 h-5 bg-gray-100 animate-wave"
              style={{ animationDelay: "300ms" }}
            ></div>
          </div>
        </DialogHeader>
        <DialogHeader className="pb-2">
          <DialogTitle className="text-center text-black">We zijn er mee bezig...</DialogTitle>
          <DialogDescription className="text-center text-black">
            Even geduld a.u.b.
          </DialogDescription>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
};

export default Loader;