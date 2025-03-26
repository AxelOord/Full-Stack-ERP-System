import React from "react";

interface LoaderProps {
  isLoading: boolean;
}

const Loader: React.FC<LoaderProps> = ({ isLoading }) => {
  if (!isLoading) return null;

  // Use a fixed position overlay that appears immediately
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center"
      // In the overlay div's style object, update these properties:
      // Update the overlay div's style to make dots more visible
      style={{
        backdropFilter: 'blur(2px)',
        backgroundColor: 'rgba(255, 255, 255, 0.25)',
        backgroundImage: 'radial-gradient(circle, rgba(150,150,150,0.35) 2px, transparent 2px)',
        backgroundSize: '60px 60px',
        boxShadow: 'inset 0 0 0 1px rgba(255, 255, 255, 0.1)',
        border: '1px solid rgba(255, 255, 255, 0.18)'
      }}>
      <div className="flex items-center justify-center space-x-2">
        <div
          className="w-6 h-6 bg-turquoise animate-wave"
          style={{ animationDelay: "0ms" }}
        ></div>
        <div
          className="w-6 h-6 bg-orange animate-wave"
          style={{ animationDelay: "100ms" }}
        ></div>
        <div
          className="w-6 h-6 bg-blue animate-wave"
          style={{ animationDelay: "200ms" }}
        ></div>
        <div
          className="w-6 h-6 bg-gray-300 animate-wave"
          style={{ animationDelay: "300ms" }}
        ></div>
      </div>
    </div>
  );
};

export default Loader;