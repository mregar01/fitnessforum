import React from 'react';
import './App.css';

interface RenderHTMLProps {
  HTML?: string;
}

const RenderHTML: React.FC<RenderHTMLProps> = ({ HTML }) => {
  if (!HTML) {
    return null;
  }

  return <span dangerouslySetInnerHTML={{ __html: HTML }}></span>;
};

export default RenderHTML;
