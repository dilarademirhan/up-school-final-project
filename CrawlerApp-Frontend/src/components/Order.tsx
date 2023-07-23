import React, { useState } from 'react';

const Order = () => {
  const [productType, setProductType] = useState('');
  const [productCount, setProductCount] = useState('');

  const handleProductTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setProductType(e.target.value);
  };

  const handleProductCountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setProductCount(e.target.value);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    console.log(productType)
    console.log(productCount)
    setProductType('');
    setProductCount('');
  };

  return (
    <>
    <h2>Order</h2>
    <form onSubmit={handleSubmit}>
      <div>
        <label>Product Type: </label>
        <br></br>
        <select value={productType} onChange={handleProductTypeChange}>
          <option value="">Select an option</option>
          <option value="all">All</option>
          <option value="onSale">On Sale</option>
          <option value="regularPrice">Regular Price Products</option>
        </select>
      </div>
      <br></br>
      <div>
          <label>Number of Products: </label>
          <br></br>
          <input
            type="number" min={0}
            value={productCount}
            onChange={handleProductCountChange}
          />
      </div>
      <br></br>
      <button type="submit">Submit</button>
    </form>
    </>
  );
};

export default Order;
