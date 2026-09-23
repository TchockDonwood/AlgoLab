export default function Button({
    children,
    variant = "primary",
    size = "medium",
    loading = false,
    disabled = false,
    className = "",
    type = "button",
    ...props
}) {
    const classes = [
        "ui-button",
        `ui-button--${variant}`,
        `ui-button--${size}`,
        loading ? "ui-button--loading" : "",
        className,
    ]
        .filter(Boolean)
        .join(" ");

    return (
        <button
            type={type}
            className={classes}
            disabled={disabled || loading}
            {...props}
        >
            {loading ? "Загрузка..." : children}
        </button>
    );
}