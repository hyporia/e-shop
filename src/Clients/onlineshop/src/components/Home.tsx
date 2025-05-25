import React from "react";
import { Link } from "react-router-dom";

const Home = (): JSX.Element => {
    // Mock featured products (in a real app, these would come from API/props)
    const featuredProducts = [
        {
            id: "1",
            name: "Minimalist Desk Lamp",
            price: 59.99,
            image: "https://picsum.photos/seed/product1/800/800",
        },
        {
            id: "2",
            name: "Modern Lounge Chair",
            price: 299.99,
            image: "https://picsum.photos/seed/product2/800/800",
        },
        {
            id: "3",
            name: "Ceramic Coffee Mug",
            price: 24.99,
            image: "https://picsum.photos/seed/product3/800/800",
        },
        {
            id: "4",
            name: "Leather Notebook",
            price: 39.99,
            image: "https://picsum.photos/seed/product4/800/800",
        },
    ];

    // Categories with images
    const categories = [
        {
            name: "Home Decor",
            image: "https://picsum.photos/seed/cat1/600/600",
            link: "/products?category=home-decor",
        },
        {
            name: "Furniture",
            image: "https://picsum.photos/seed/cat2/600/600",
            link: "/products?category=furniture",
        },
        {
            name: "Kitchenware",
            image: "https://picsum.photos/seed/cat3/600/600",
            link: "/products?category=kitchenware",
        },
    ];

    return (
        <div className="space-y-16 fade-in">
            {/* Hero Section */}
            <section className="relative overflow-hidden rounded-2xl bg-gray-900 text-white">
                <img
                    src="https://picsum.photos/seed/hero/1920/1080"
                    alt="Modern interior with minimalist design"
                    className="absolute inset-0 w-full h-full object-cover opacity-60"
                />
                <div className="relative z-10 py-24 px-6 md:px-12 lg:px-16 max-w-4xl">
                    <h1 className="text-4xl md:text-5xl font-bold mb-6 tracking-tight">
                        Minimalist Design for Modern Living
                    </h1>
                    <p className="text-lg md:text-xl text-gray-100 mb-8 max-w-lg">
                        Discover our collection of thoughtfully designed
                        products that blend form and function for your everyday
                        needs.
                    </p>
                    <div className="flex flex-col sm:flex-row gap-4">
                        <Link to="/products" className="btn btn-primary btn-lg">
                            Shop Now
                        </Link>
                        <Link
                            to="/about"
                            className="btn btn-outline text-white border-white btn-lg hover:bg-white/20"
                        >
                            Learn More
                        </Link>
                    </div>
                </div>
            </section>

            {/* Featured Products Section */}
            <section>
                <div className="flex items-center justify-between mb-8">
                    <h2 className="text-2xl font-bold">Featured Products</h2>
                    <Link
                        to="/products"
                        className="text-primary hover:underline flex items-center"
                    >
                        View all
                        <svg
                            className="w-4 h-4 ml-1"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M9 5l7 7-7 7"
                            />
                        </svg>
                    </Link>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                    {featuredProducts.map((product) => (
                        <div key={product.id} className="card group">
                            <Link
                                to={`/products/${product.id}`}
                                className="block relative aspect-square overflow-hidden"
                            >
                                <img
                                    src={product.image}
                                    alt={product.name}
                                    className="object-cover w-full h-full transition-all duration-500 ease-out group-hover:scale-105"
                                />
                            </Link>
                            <div className="p-4">
                                <h3 className="font-medium mb-1 truncate">
                                    <Link
                                        to={`/products/${product.id}`}
                                        className="hover:text-primary"
                                    >
                                        {product.name}
                                    </Link>
                                </h3>
                                <div className="text-primary font-semibold">
                                    ${product.price.toFixed(2)}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </section>

            {/* Categories Section */}
            <section>
                <h2 className="text-2xl font-bold mb-8">Shop by Category</h2>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    {categories.map((category, index) => (
                        <Link
                            key={index}
                            to={category.link}
                            className="relative overflow-hidden rounded-lg group aspect-[4/3]"
                        >
                            <img
                                src={category.image}
                                alt={category.name}
                                className="absolute inset-0 w-full h-full object-cover transition-transform duration-500 ease-out group-hover:scale-105"
                            />
                            <div className="absolute inset-0 bg-black bg-opacity-30 transition-opacity group-hover:bg-opacity-40 flex items-center justify-center">
                                <h3 className="text-white text-2xl font-bold">
                                    {category.name}
                                </h3>
                            </div>
                        </Link>
                    ))}
                </div>
            </section>

            {/* Newsletter Section */}
            <section className="bg-gray-50 rounded-2xl p-8 md:p-12">
                <div className="max-w-2xl mx-auto text-center">
                    <h2 className="text-2xl font-bold mb-4">
                        Join Our Community
                    </h2>
                    <p className="text-text-light mb-6">
                        Subscribe to our newsletter for exclusive offers, design
                        inspiration, and early access to new products.
                    </p>
                    <form className="flex flex-col sm:flex-row gap-3 max-w-md mx-auto">
                        <input
                            type="email"
                            placeholder="Enter your email"
                            className="form-input flex-1"
                            required
                        />
                        <button type="submit" className="btn btn-primary">
                            Subscribe
                        </button>
                    </form>
                </div>
            </section>

            {/* Value Propositions Section */}
            <section className="grid grid-cols-1 md:grid-cols-3 gap-6 border-t border-b border-gray-200 py-12">
                {[
                    {
                        title: "Free Shipping",
                        description: "On orders over $50",
                        icon: (
                            <svg
                                className="w-6 h-6"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={1.5}
                                    d="M5 8h14M5 8a2 2 0 100-4 2 2 0 000 4zm0 0v10a2 2 0 002 2h10a2 2 0 002-2V8m-9 4h4"
                                />
                            </svg>
                        ),
                    },
                    {
                        title: "30-Day Returns",
                        description: "Hassle-free returns",
                        icon: (
                            <svg
                                className="w-6 h-6"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={1.5}
                                    d="M16 15v-1a4 4 0 00-4-4H8m0 0l3 3m-3-3l3-3m9 14V5a2 2 0 00-2-2H6a2 2 0 00-2 2v16l4-2 4 2 4-2 4 2z"
                                />
                            </svg>
                        ),
                    },
                    {
                        title: "Secure Payment",
                        description: "100% secure checkout",
                        icon: (
                            <svg
                                className="w-6 h-6"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={1.5}
                                    d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z"
                                />
                            </svg>
                        ),
                    },
                ].map((item, index) => (
                    <div
                        key={index}
                        className="flex flex-col items-center text-center p-4"
                    >
                        <div className="w-12 h-12 bg-primary-light rounded-full flex items-center justify-center text-primary mb-4">
                            {item.icon}
                        </div>
                        <h3 className="font-semibold mb-1">{item.title}</h3>
                        <p className="text-text-light text-sm">
                            {item.description}
                        </p>
                    </div>
                ))}
            </section>
        </div>
    );
};

export default Home;
