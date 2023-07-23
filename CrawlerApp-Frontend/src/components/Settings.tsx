import React, { useState } from 'react';

function Settings() {
  const [selectedOption, setSelectedOption] = useState<string>('');

  const handleOptionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedOption(event.target.value);
  };

  return (
    <>
      <h2>Notification Settings</h2>
      <p>Select Notification Type: </p>
      <div>
        <label>
          <input
            type="radio"
            value="Email"
            checked={selectedOption === "Email"}
            onChange={handleOptionChange}
          />
          E-Mail
        </label>
      </div>
      <div>
        <label>
          <input
            type="radio"
            value="InApp"
            checked={selectedOption === "InApp"}
            onChange={handleOptionChange}
          />
          In App Notification
        </label>
      </div>
      <div>
        <label>
          <input
            type="radio"
            value="None"
            checked={selectedOption === "None"}
            onChange={handleOptionChange}
          />
          None
        </label>
      </div>
    </>
  );
}

export default Settings;
