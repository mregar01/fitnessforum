import React from 'react';
import './App.css';
import { Link } from 'react-router-dom';

interface TagListProps {
  string?: string;
  className?: string;
}

const TagList: React.FC<TagListProps> = ({ string }) => {
  const items = string?.split(/[<>]+/).filter((item) => item.trim() !== '');

  return (
    <div>
      {items &&
        items.map((item, index) => (
          <Link to={`/posts/tag/${item}`} key={index}>
            <button className='btn btn-secondary m-1 btn-sm'>
              {item}
            </button>
          </Link>
        ))}
    </div>
  );
};

export default TagList;
