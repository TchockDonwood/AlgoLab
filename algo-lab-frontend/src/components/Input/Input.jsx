export default function Input({
  label,
  value,
  onChange,
  type = "number",
  error,
  hint,
  className = "",
  ...props
}) {
  return (
    <div className={`ui-input-group ${className}`}>
      {label && (
        <label className="ui-input-label">
          {label}
        </label>
      )}

      <input
        className={`ui-input ${error ? "ui-input--error" : ""}`}
        type={type}
        value={value}
        onChange={onChange}
        {...props}
      />

      {error && (
        <div className="ui-input-error">
          {error}
        </div>
      )}

      {!error && hint && (
        <div className="ui-input-hint">
          {hint}
        </div>
      )}
    </div>
  );
}