import React from 'react';

interface PrintDateProps {
  className?: string;
  dateString?: string;
  type: 'post' | 'user';
  monthType?: "numeric" | "2-digit" | "long" | "short" | "narrow";
}

const PrintDate: React.FC<PrintDateProps> = ({ className, dateString, type, monthType }) => {
  if (!dateString) {
    return null;
  } else {
    const dt = new Date(dateString);
    const day = dt.getDate();
    const year = dt.getFullYear();
    const hours = dt.getHours();
    const minutes = dt.getMinutes();

    if (type === 'post') {
      return (
        <small className={className}>
          {" "}
          {dt.toLocaleString('en-US', { month: 'long' })} {day}, {year} at {hours}:{minutes}
          <br />
        </small>
      );
    } else if (type === 'user') {
      return (
        <small className={className}>
          {dt.toLocaleString('en-US', { month: monthType })} {day}, {year}{' '}
        </small>
      );
    } else {
      return null;
    }
  }
};

export default PrintDate;
