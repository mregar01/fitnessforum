import React from 'react';
import './App.css'


interface LoadingOverlayProps {
  message: string;
}


const LoadingOverlay: React.FC<LoadingOverlayProps> = ({ message }) => {
  return (
    <div className="loading-overlay">
      <div className="loading-message">{message}</div>
      <div className="loader"></div>
    </div>
  );
};


export default LoadingOverlay;