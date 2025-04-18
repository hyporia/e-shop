import React, { useState } from "react";
import { Link } from "react-router-dom";

// Cart item type definition
interface CartItem {
    id: string;
    name: string;
    price: number;
    quantity: number;
    image: string;
}

// Mock initial cart items (in a real app, these would come from a cart context or state)
const initialCartItems: CartItem[] = [
    {
        id: "1",
        name: "Minimalist Desk Lamp",
        price: 59.99,
        quantity: 1,
        image: "https://picsum.photos/seed/product1/200/200",
    },
    {
        id: "2",
        name: "Modern Lounge Chair",
        price: 299.99,
        quantity: 1,
        image: "https://picsum.photos/seed/product2/200/200",
    },
];

const ShoppingCart = (): JSX.Element => {
    const [cartItems, setCartItems] = useState<CartItem[]>(initialCartItems);

    // Calculate cart totals
    const subtotal = cartItems.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0
    );
    const shipping = subtotal > 100 ? 0 : 10;
    const tax = subtotal * 0.08; // 8% tax
    const total = subtotal + shipping + tax;

    // Handle quantity change
    const updateQuantity = (id: string, newQuantity: number) => {
        if (newQuantity < 1) return;

        setCartItems(
            cartItems.map((item) =>
                item.id === id ? { ...item, quantity: newQuantity } : item
            )
        );
    };

    // Remove item from cart
    const removeItem = (id: string) => {
        setCartItems(cartItems.filter((item) => item.id !== id));
    };

    // If cart is empty
    if (cartItems.length === 0) {
        return (
            <div className="max-w-2xl mx-auto py-8 text-center">
                <div className="bg-surface rounded-xl shadow-sm p-8">
                    <svg
                        className="mx-auto h-16 w-16 text-gray-400"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="1.5"
                            d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"
                        />
                    </svg>
                    <h2 className="mt-4 text-2xl font-semibold">
                        Your cart is empty
                    </h2>
                    <p className="mt-2 text-text-light">
                        Looks like you haven't added anything to your cart yet.
                    </p>
                    <div className="mt-6">
                        <Link
                            to="/products"
                            className="btn btn-primary py-2 px-4"
                        >
                            Continue Shopping
                        </Link>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="max-w-5xl mx-auto py-8 fade-in">
            <h1 className="text-3xl font-bold mb-8">Shopping Cart</h1>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                {/* Cart Items */}
                <div className="lg:col-span-2">
                    <div className="bg-surface rounded-xl shadow-sm overflow-hidden">
                        <div className="p-6 border-b border-gray-200">
                            <h2 className="text-xl font-semibold">
                                Items ({cartItems.length})
                            </h2>
                        </div>

                        <ul className="divide-y divide-gray-200">
                            {cartItems.map((item) => (
                                <li key={item.id} className="p-6">
                                    <div className="flex flex-col sm:flex-row items-start gap-4">
                                        {/* Product image */}
                                        <div className="w-24 h-24 bg-gray-100 rounded-md overflow-hidden flex-shrink-0">
                                            <img
                                                src={item.image}
                                                alt={item.name}
                                                className="w-full h-full object-cover"
                                            />
                                        </div>

                                        {/* Product details */}
                                        <div className="flex-1">
                                            <div className="flex justify-between">
                                                <h3 className="font-medium text-lg">
                                                    <Link
                                                        to={`/products/${item.id}`}
                                                        className="hover:text-primary"
                                                    >
                                                        {item.name}
                                                    </Link>
                                                </h3>
                                                <div className="font-semibold">
                                                    $
                                                    {(
                                                        item.price *
                                                        item.quantity
                                                    ).toFixed(2)}
                                                </div>
                                            </div>
                                            <div className="text-primary">
                                                ${item.price.toFixed(2)}
                                            </div>

                                            {/* Quantity controls and remove button */}
                                            <div className="flex items-center justify-between mt-4">
                                                <div className="flex items-center">
                                                    <button
                                                        className="w-8 h-8 rounded-md bg-gray-100 flex items-center justify-center"
                                                        onClick={() =>
                                                            updateQuantity(
                                                                item.id,
                                                                item.quantity -
                                                                    1
                                                            )
                                                        }
                                                    >
                                                        <svg
                                                            className="w-4 h-4"
                                                            fill="none"
                                                            stroke="currentColor"
                                                            viewBox="0 0 24 24"
                                                        >
                                                            <path
                                                                strokeLinecap="round"
                                                                strokeLinejoin="round"
                                                                strokeWidth={2}
                                                                d="M20 12H4"
                                                            />
                                                        </svg>
                                                    </button>

                                                    <input
                                                        type="number"
                                                        className="w-12 h-8 mx-1 text-center border border-gray-200 rounded-md"
                                                        min="1"
                                                        value={item.quantity}
                                                        onChange={(e) =>
                                                            updateQuantity(
                                                                item.id,
                                                                parseInt(
                                                                    e.target
                                                                        .value
                                                                ) || 1
                                                            )
                                                        }
                                                    />

                                                    <button
                                                        className="w-8 h-8 rounded-md bg-gray-100 flex items-center justify-center"
                                                        onClick={() =>
                                                            updateQuantity(
                                                                item.id,
                                                                item.quantity +
                                                                    1
                                                            )
                                                        }
                                                    >
                                                        <svg
                                                            className="w-4 h-4"
                                                            fill="none"
                                                            stroke="currentColor"
                                                            viewBox="0 0 24 24"
                                                        >
                                                            <path
                                                                strokeLinecap="round"
                                                                strokeLinejoin="round"
                                                                strokeWidth={2}
                                                                d="M12 4v16m8-8H4"
                                                            />
                                                        </svg>
                                                    </button>
                                                </div>

                                                <button
                                                    className="text-gray-400 hover:text-error transition-colors"
                                                    onClick={() =>
                                                        removeItem(item.id)
                                                    }
                                                >
                                                    <span className="sr-only">
                                                        Remove
                                                    </span>
                                                    <svg
                                                        className="w-5 h-5"
                                                        fill="none"
                                                        stroke="currentColor"
                                                        viewBox="0 0 24 24"
                                                    >
                                                        <path
                                                            strokeLinecap="round"
                                                            strokeLinejoin="round"
                                                            strokeWidth={1.5}
                                                            d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                                                        />
                                                    </svg>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>

                {/* Order Summary */}
                <div className="lg:col-span-1">
                    <div className="bg-surface rounded-xl shadow-sm p-6">
                        <h2 className="text-xl font-semibold mb-4">
                            Order Summary
                        </h2>

                        <div className="space-y-3 text-sm">
                            <div className="flex justify-between">
                                <span className="text-text-light">
                                    Subtotal
                                </span>
                                <span>${subtotal.toFixed(2)}</span>
                            </div>
                            <div className="flex justify-between">
                                <span className="text-text-light">
                                    Shipping
                                </span>
                                <span>
                                    {shipping > 0
                                        ? `$${shipping.toFixed(2)}`
                                        : "Free"}
                                </span>
                            </div>
                            <div className="flex justify-between">
                                <span className="text-text-light">Tax</span>
                                <span>${tax.toFixed(2)}</span>
                            </div>
                            <div className="border-t border-gray-200 my-4 pt-4">
                                <div className="flex justify-between font-semibold">
                                    <span>Total</span>
                                    <span>${total.toFixed(2)}</span>
                                </div>
                            </div>
                        </div>

                        <div className="mt-6">
                            <button className="btn btn-primary w-full py-3">
                                Proceed to Checkout
                            </button>
                        </div>

                        <div className="mt-4">
                            <Link
                                to="/products"
                                className="text-primary hover:underline flex items-center justify-center text-sm"
                            >
                                <svg
                                    className="w-4 h-4 mr-1"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d="M10 19l-7-7m0 0l7-7m-7 7h18"
                                    />
                                </svg>
                                Continue Shopping
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ShoppingCart;
