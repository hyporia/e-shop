import { Link } from "react-router-dom";
import Products from "../products/Products";
import "./Home.css";

const Home = (): JSX.Element => {
    return (
        <div className="home fade-in">
            {/* Hero section */}
            <section className="bg-primary-light py-12 md:py-24 rounded-2xl mb-12">
                <div className="container mx-auto px-4 md:px-8">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-8 items-center">
                        <div className="text-center md:text-left">
                            <h1 className="text-4xl md:text-5xl font-bold tracking-tight mb-4 text-primary">
                                Modern Shopping Experience
                            </h1>
                            <p className="text-lg mb-6 text-gray-700 max-w-md mx-auto md:mx-0">
                                Discover our curated collection of premium
                                products with a simplified shopping experience.
                            </p>
                            <div className="flex flex-col sm:flex-row gap-4 justify-center md:justify-start">
                                <Link
                                    to="/products"
                                    className="btn btn-primary btn-lg"
                                >
                                    Browse Products
                                </Link>
                                <Link
                                    to="/categories"
                                    className="btn btn-outline btn-lg"
                                >
                                    View Categories
                                </Link>
                            </div>
                        </div>
                        <div className="hidden md:block">
                            <div className="aspect-square bg-white rounded-xl shadow-lg p-4 transform rotate-3 relative overflow-hidden">
                                <img
                                    src="https://picsum.photos/seed/homeproduct/600/600"
                                    alt="Featured product"
                                    className="w-full h-full object-cover rounded-lg"
                                />
                                <div className="absolute top-4 right-4 bg-primary text-white text-sm py-1 px-3 rounded-full font-medium">
                                    New Arrival
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* Features section */}
            <section className="py-12 mb-12">
                <div className="container mx-auto px-4">
                    <h2 className="text-2xl font-semibold text-center mb-10">
                        Why Shop With Us
                    </h2>

                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        {/* Feature 1 */}
                        <div className="bg-surface rounded-xl p-6 text-center shadow-sm hover:shadow-md transition-shadow">
                            <div className="bg-primary-light w-14 h-14 rounded-full flex items-center justify-center mx-auto mb-4">
                                <svg
                                    className="w-6 h-6 text-primary"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth="2"
                                        d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                                    ></path>
                                </svg>
                            </div>
                            <h3 className="text-lg font-semibold mb-2">
                                Quality Products
                            </h3>
                            <p className="text-text-light">
                                We curate only the highest quality products for
                                our customers.
                            </p>
                        </div>

                        {/* Feature 2 */}
                        <div className="bg-surface rounded-xl p-6 text-center shadow-sm hover:shadow-md transition-shadow">
                            <div className="bg-primary-light w-14 h-14 rounded-full flex items-center justify-center mx-auto mb-4">
                                <svg
                                    className="w-6 h-6 text-primary"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth="2"
                                        d="M13 10V3L4 14h7v7l9-11h-7z"
                                    ></path>
                                </svg>
                            </div>
                            <h3 className="text-lg font-semibold mb-2">
                                Fast Shipping
                            </h3>
                            <p className="text-text-light">
                                Get your products delivered to your doorstep
                                within 2-3 business days.
                            </p>
                        </div>

                        {/* Feature 3 */}
                        <div className="bg-surface rounded-xl p-6 text-center shadow-sm hover:shadow-md transition-shadow">
                            <div className="bg-primary-light w-14 h-14 rounded-full flex items-center justify-center mx-auto mb-4">
                                <svg
                                    className="w-6 h-6 text-primary"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth="2"
                                        d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z"
                                    ></path>
                                </svg>
                            </div>
                            <h3 className="text-lg font-semibold mb-2">
                                Secure Payment
                            </h3>
                            <p className="text-text-light">
                                Your payments are secure with our encrypted
                                payment gateway.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            {/* Featured Categories */}
            <section className="py-6 mb-12">
                <div className="container mx-auto px-4">
                    <h2 className="text-2xl font-semibold mb-8">
                        Shop by Category
                    </h2>

                    <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
                        {[
                            "Electronics",
                            "Clothing",
                            "Home & Kitchen",
                            "Beauty",
                        ].map((category, index) => (
                            <div
                                key={index}
                                className="relative rounded-lg overflow-hidden aspect-[4/3] group"
                            >
                                <img
                                    src={`https://picsum.photos/seed/${category}/600/450`}
                                    alt={category}
                                    className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110"
                                />
                                <div className="absolute inset-0 bg-gradient-to-t from-black/60 to-transparent flex items-end p-4">
                                    <h3 className="text-white font-semibold">
                                        {category}
                                    </h3>
                                </div>
                                <Link
                                    to={`/category/${category.toLowerCase()}`}
                                    className="absolute inset-0"
                                    aria-label={`Shop ${category}`}
                                ></Link>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Featured Products */}
            <section className="py-6">
                <div className="container mx-auto px-4">
                    <h2 className="text-2xl font-semibold mb-8">
                        Featured Products
                    </h2>
                    <Products />
                </div>
            </section>

            {/* Newsletter */}
            <section className="py-12 my-12 bg-gray-50 rounded-2xl">
                <div className="container mx-auto px-4 text-center max-w-2xl">
                    <h2 className="text-2xl font-semibold mb-3">
                        Join Our Newsletter
                    </h2>
                    <p className="text-text-light mb-6">
                        Subscribe to our newsletter to receive updates on new
                        products, special offers, and more.
                    </p>
                    <form className="flex flex-col sm:flex-row gap-2">
                        <input
                            type="email"
                            placeholder="Enter your email"
                            className="form-input flex-grow py-3"
                            required
                        />
                        <button
                            type="submit"
                            className="btn btn-primary whitespace-nowrap"
                        >
                            Subscribe
                        </button>
                    </form>
                </div>
            </section>
        </div>
    );
};

export default Home;
